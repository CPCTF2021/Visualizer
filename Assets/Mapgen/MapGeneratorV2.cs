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
        public int simulationGroupSize = 1;

	    [Header("地形生成")]
        public Vector2 continentShapeSeed = Vector2.zero;
        [Range(-1, 1)]
        public float continentHeightOffset = 0f;
        public float continentSharpness = 1f;
        public float continentNoiseScale = 1f;

        public float oceanFloorSmoothing = 1f;
        public float oceanDepthMultiplier = 0f;

        public Vector2 mountainMaskSeed = Vector2.zero;
        [Range(0, 1)]
        public float mountainMaskGain = 1f;
        public float mountainMaskScale = 1f;
        [Range(-1, 1)]
        public float mountainMaskOffset = 1f;

        public Vector2 mountainNoiseSeed = Vector2.zero;
        public float mountainNoiseScale = 1f;
        public float mountainHeightScale = 1f;
        public float mountainSharpness = 1f;

        public float continentScale = 0.5f;
        public float noiseBlendScale = 3.0f;

        [Header("地形彩色")]
        public Texture heightColorTexture;
        public Color glassColor;
        public float glassNormalSpread = 2.0f;
        public float glassHeightSpread = 0.8f;
    }
    // [ExecuteInEditMode]
    public class MapGeneratorV2: MonoBehaviour
    {
        [SerializeField]
        ComputeShader meshBuilder;
        [SerializeField]
        Setting setting;

        int buildMeshKernel;
        int renderTextureKernel;
        MeshFilter meshFilter;
        Mesh originalMesh;

        void FlatShading ()
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            Mesh mesh = Instantiate (mf.sharedMesh) as Mesh;
            mf.sharedMesh = mesh;

            Vector3[] oldVerts = mesh.vertices;
            int[] triangles = mesh.triangles;
            Vector3[] vertices = new Vector3[triangles.Length];

            for (int i = 0; i < triangles.Length; i++) 
            {
                vertices[i] = oldVerts[triangles[i]];
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        public void Start()
        {
            FlatShading();
            meshFilter = GetComponent<MeshFilter>();
            originalMesh = meshFilter.sharedMesh;
            StartCoroutine("Loop");
        }
        IEnumerator Loop()
        {
            while(true)
            {
                MeshUpdate();
                yield return new WaitForSeconds(0.2f);
            }
        }
        public void MeshUpdate()
        {
            Mesh mesh = Instantiate(originalMesh);

            buildMeshKernel = meshBuilder.FindKernel("BuildMesh");
            renderTextureKernel = meshBuilder.FindKernel("RenderTexture");

            List<Vector3> vertices = new List<Vector3>();
            mesh.GetVertices(vertices);
            List<Vector2> uvs = new List<Vector2>();
            mesh.GetUVs(0, uvs);

            int maxVertexCount = vertices.Count;
            int blockComputeCount = Mathf.CeilToInt(maxVertexCount / setting.simulationBlockSize * setting.simulationGroupSize);

            ComputeBuffer vertexBuffer = new ComputeBuffer(maxVertexCount, Marshal.SizeOf(typeof(Vector3)));
            vertexBuffer.SetData(vertices.ToArray());
            ComputeBuffer uvBuffer = new ComputeBuffer(maxVertexCount, Marshal.SizeOf(typeof(Vector2)));
            uvBuffer.SetData(uvs.ToArray());
            ComputeBuffer heightBuffer = new ComputeBuffer(maxVertexCount, sizeof(float));
            ComputeBuffer normalBuffer = new ComputeBuffer(maxVertexCount, Marshal.SizeOf(typeof(Vector3)));
            ComputeBuffer colorBuffer = new ComputeBuffer(maxVertexCount, Marshal.SizeOf(typeof(Color)));


            meshBuilder.SetInt("maxVertexCount", maxVertexCount);
            meshBuilder.SetInt("blockComputeCount", blockComputeCount);
            meshBuilder.SetVector("continentShapeSeed", setting.continentShapeSeed);
            meshBuilder.SetFloat("continentNoiseScale", setting.continentNoiseScale);
            meshBuilder.SetFloat("continentHeightOffset", setting.continentHeightOffset);
            meshBuilder.SetFloat("continentSharpness", setting.continentSharpness);

            meshBuilder.SetFloat("oceanFloorSmoothing", setting.oceanFloorSmoothing);
            meshBuilder.SetFloat("oceanDepthMultiplier", setting.oceanDepthMultiplier);

            meshBuilder.SetVector("mountainMaskSeed", setting.mountainMaskSeed);
            meshBuilder.SetFloat("mountainMaskGain", setting.mountainMaskGain);
            meshBuilder.SetFloat("mountainMaskScale", setting.mountainMaskScale);
            meshBuilder.SetFloat("mountainMaskOffset", setting.mountainMaskOffset);

            meshBuilder.SetVector("mountainNoiseSeed", setting.mountainNoiseSeed);
            meshBuilder.SetFloat("mountainNoiseScale", setting.mountainNoiseScale);
            meshBuilder.SetFloat("mountainHeightScale", setting.mountainHeightScale);
            meshBuilder.SetFloat("mountainSharpness", setting.mountainSharpness);

            meshBuilder.SetFloat("continentScale", setting.continentScale);
            meshBuilder.SetFloat("noiseBlendScale", setting.noiseBlendScale);
            meshBuilder.SetBuffer(buildMeshKernel, "_VertexBuffer", vertexBuffer);
            meshBuilder.SetBuffer(buildMeshKernel, "_HeightBuffer", heightBuffer);
            meshBuilder.SetBuffer(buildMeshKernel, "_UVBuffer", uvBuffer);

            meshBuilder.Dispatch(buildMeshKernel, setting.simulationGroupSize, 1, 1);

            Vector3[] newVertices = new Vector3[maxVertexCount];
            vertexBuffer.GetData(newVertices);
            Vector2[] newUVs = new Vector2[maxVertexCount];
            uvBuffer.GetData(newUVs);

            mesh.SetVertices(newVertices);
            mesh.SetUVs(0, newUVs);
            mesh.RecalculateNormals();
            List<Vector3> newNormals = new List<Vector3>();
            mesh.GetNormals(newNormals);
            normalBuffer.SetData(newNormals);

            // 地形色生成
            meshBuilder.SetTexture(renderTextureKernel, "_HeightColorTexture", setting.heightColorTexture);
            meshBuilder.SetVector("glassColor", setting.glassColor);
            meshBuilder.SetFloat("glassNormalSpread", setting.glassNormalSpread);
            meshBuilder.SetFloat("glassHeightSpread", setting.glassHeightSpread);

            meshBuilder.SetBuffer(renderTextureKernel, "_VertexBuffer", vertexBuffer);
            meshBuilder.SetBuffer(renderTextureKernel, "_HeightBuffer", heightBuffer);
            meshBuilder.SetBuffer(renderTextureKernel, "_UVBuffer", uvBuffer);
            meshBuilder.SetBuffer(renderTextureKernel, "_NormalBuffer", normalBuffer);
            meshBuilder.SetBuffer(renderTextureKernel, "_ColorBuffer", colorBuffer);

            meshBuilder.Dispatch(renderTextureKernel, setting.simulationGroupSize, 1, 1);

            Color[] newColors = new Color[maxVertexCount];
            colorBuffer.GetData(newColors);

            mesh.SetColors(newColors);

            meshFilter.mesh = mesh;
            
            vertexBuffer.Release();
            uvBuffer.Release();
            heightBuffer.Release();
            normalBuffer.Release();
            colorBuffer.Release();
        }
    }
}
