using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Atomosphere : MonoBehaviour {
    public Material mat;
    [SerializeField]
    Texture2D spectreTex;
    [SerializeField]
    Vector3 center = Vector3.zero;
    [SerializeField]
    float atomosphereRadius = 0.6f;
    [SerializeField]
    float planetRadius = 0.5f;
    void Start () 
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;

    }

    public void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        mat.SetVector("_Center", center);
        mat.SetFloat("_AtomosphereRadius", atomosphereRadius);
        mat.SetFloat("_PlanetRadius", planetRadius);
        mat.SetTexture("_Spectre", spectreTex);
        Graphics.Blit(source, dest, mat);
    }
}