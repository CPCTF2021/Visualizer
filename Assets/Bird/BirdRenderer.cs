using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bird {
    public class BirdRenderer : MonoBehaviour
    {
        BirdSimulation birdSimulation;

        public Mesh instanceMesh;
        public Material instanceRenderMaterial;
        uint[] args = new uint[5] {0, 0, 0, 0, 0};
        ComputeBuffer argsBuffer;

        void Start() {
            birdSimulation = GetComponent<BirdSimulation>();
            argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        }

        void Update() {
            if(
                instanceRenderMaterial == null ||
                birdSimulation == null ||
                !SystemInfo.supportsInstancing
            ) return;

            uint numIndices = (instanceMesh != null) ? (uint)instanceMesh.GetIndexCount(0) : 0;
            args[0] = numIndices;
            args[1] = (uint)birdSimulation.setting.birdNum;
            argsBuffer.SetData(args);

            instanceRenderMaterial.SetBuffer("_BirdDataBuffer", birdSimulation.birdsBuffer);
            Vector3 size = new Vector3(1f, 1f, 1f) * birdSimulation.setting.planetRadius;
            Bounds bounds = new Bounds(
                Vector3.zero,
                size
            );
            Graphics.DrawMeshInstancedIndirect(
                instanceMesh,
                0,
                instanceRenderMaterial,
                bounds,
                argsBuffer
            );
        }

        void OnDestroy() {
            if(argsBuffer != null) {
                argsBuffer.Release();
                argsBuffer = null;
            }
        }
    }
}
