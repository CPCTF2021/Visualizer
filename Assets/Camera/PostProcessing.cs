using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraLib
{
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField]
        Material filter;

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src,dest,filter);
        }
    }
}
