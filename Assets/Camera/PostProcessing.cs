using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CameraScripts
{
    [ExecuteInEditMode]
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField]
        Material filter;

        Material material;

        void Start()
        {
            #if UNITY_EDITOR
            if(EditorApplication.isPlaying) {
            #endif
                material = Instantiate(filter) as Material;
            #if UNITY_EDITOR
            } else {
                material = filter;
            }
            #endif
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src,dest,material);
        }

        public void SetProgress(float progress) 
        {
            material.SetFloat("_Progress", progress);
        }
    }
}
