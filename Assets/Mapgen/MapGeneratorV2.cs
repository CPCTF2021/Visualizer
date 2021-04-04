using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Map
{
    [Serializable]
    class Setting
    {
        public int simulationBlockSize = 256;
        public float continentScale = 0.5f;
        public float noiseScale = 1f;
        public float noiseBlendScale = 3.0f;
    }
    [ExecuteInEditMode]
    public class MapGeneratorV2: MonoBehaviour
    {
        [SerializeField]
        ComputeShader meshBuilder;
        [SerializeField]
        Setting setting;

        int buildMeshKernel;
        MeshFilter meshFilter;
        Mesh originalMesh;

        public void ResetOriginalMesh()
        {
            meshFilter = GetComponent<MeshFilter>();
            originalMesh = meshFilter.sharedMesh;
        }
    [ContextMenu("BuildMesh")]
        public void BuildMesh()
        {
            Mesh mesh = meshFilter.mesh;

            buildMeshKernel = meshBuilder.FindKernel("BuildMesh");

            List<Vector3> vertices = new List<Vector3>();
            mesh.GetVertices(vertices);

            int maxVertexCount = vertices.Count;
            int threadGroupSize = Mathf.CeilToInt(maxVertexCount / setting.simulationBlockSize);

            ComputeBuffer vertexBuffer = new ComputeBuffer(maxVertexCount, Marshal.SizeOf(typeof(Vector3)));
            vertexBuffer.SetData(vertices.ToArray());

            meshBuilder.SetInt("maxVertexCount", maxVertexCount);
            meshBuilder.SetInt("threadGroupSize", threadGroupSize);
            meshBuilder.SetFloat("continentScale", setting.continentScale);
            meshBuilder.SetFloat("noiseScale", setting.noiseScale);
            meshBuilder.SetFloat("noiseBlendScale", setting.noiseBlendScale);
            meshBuilder.SetBuffer(buildMeshKernel, "_VertexBuffer", vertexBuffer);

            meshBuilder.Dispatch(buildMeshKernel, 1, 1, 1);

            Vector3[] newVertices = new Vector3[maxVertexCount];
            vertexBuffer.GetData(newVertices);

            
            for(int i=0;i<100;i++) Debug.Log(newVertices[i] * 1000f);

            mesh.SetVertices(newVertices);
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;

            vertexBuffer.Release();
        }
    }
}
