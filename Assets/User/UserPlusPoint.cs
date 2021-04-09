using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UserScripts
{
    public class UserPlusPoint : MonoBehaviour
    {
        Camera camera;
        bool isStart = false;
        float progress;
        Vector3 origin;
        Vector3 target;
        Vector3 control;


        void Initialize(Vector3 pos, Vector3 up)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            this.origin = camera.transform.position - up * 0.3f;
            this.control = origin + up * 0.5f;
            this.target = pos;
            isStart = true;
            progress = 0f;
            DOTween.To(() => progress, (val) => {
                progress = val;
            }, 1f, 0.3f);
        }

        void Update()
        {
            if(isStart)
            {
                float t = progress;
                float s = 1f - progress;
                Vector3 pos = origin * s * s + 2f * control * t * s + target * t * t;
                transform.position = pos;
            }
        }
    }
}
