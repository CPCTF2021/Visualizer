using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Bird {

    public class BirdSimulation : MonoBehaviour
    {
        struct Bird {
            public Vector3 position;
            public Vector3 velocity;
        }

        [System.Serializable]
        public struct SimulationSetting {
            [Range(10, 256)]
            public int birdNum;
            public float planetRadius;
            public float maxSpeed;
        }
        public SimulationSetting setting;

        public ComputeShader birdCS;
        public ComputeBuffer birdsBuffer;
        ComputeBuffer birdsVelocityBuffer;
        int birdSimulationFunction;
        int copyVelocityFunction;
        void Start()
        {
            Initialize();
        }

        void Initialize() {
            // birdSimulationFunction = birdCS.FindKernel("BirdSimulation");
            // copyVelocityFunction = birdCS.FindKernel("CopyVelocity");
            birdSimulationFunction = 0;
            copyVelocityFunction = 1;
            birdsBuffer = new ComputeBuffer(setting.birdNum, Marshal.SizeOf(typeof(Bird)));
            birdsVelocityBuffer = new ComputeBuffer(setting.birdNum, Marshal.SizeOf(typeof(Vector3)));

            Bird[] birds = new Bird[setting.birdNum];
            Vector3[] vel = new Vector3[setting.birdNum];
            for(uint i=0;i<setting.birdNum;i++) {
                birds[i].position = Random.insideUnitSphere.normalized * setting.planetRadius;
                vel[i] = birds[i].velocity = Random.insideUnitSphere * 5f;
            }

            birdsBuffer.SetData(birds);
            birdsVelocityBuffer.SetData(vel);
            birds = null;
            vel = null;
        }

        void Update()
        {
            birdCS.SetInt("_BirdNum", setting.birdNum);
            birdCS.SetInt("_SparkNum", Random.Range(0, setting.birdNum * 120));
            birdCS.SetFloat("_PlanetRadius", setting.planetRadius);
            birdCS.SetFloat("_MaxSpeed", setting.maxSpeed);
            birdCS.SetFloat("_DeltaTime", Time.deltaTime);
            birdCS.SetBuffer(birdSimulationFunction, "_BirdDataRead", birdsBuffer);
            birdCS.SetBuffer(birdSimulationFunction, "_BirdVelocityDataWrite", birdsVelocityBuffer);
            birdCS.Dispatch(birdSimulationFunction, 1, 1, 1);
            // Vector3[] birds = new Vector3[setting.birdNum];
            // birdsVelocityBuffer.GetData(birds);
            // Debug.Log(birds[1]);
            birdCS.SetBuffer(copyVelocityFunction, "_BirdDataWrite", birdsBuffer);
            birdCS.SetBuffer(copyVelocityFunction, "_BirdVelocityDataRead", birdsVelocityBuffer);
            birdCS.Dispatch(copyVelocityFunction, 1, 1, 1);
        }

        void OnDestroy() {
            if (birdsBuffer != null) {
                birdsBuffer.Release();
                birdsBuffer = null;
            }

            if (birdsVelocityBuffer != null) {
                birdsVelocityBuffer.Release();
                birdsVelocityBuffer = null;
            }
        }
    }
}
