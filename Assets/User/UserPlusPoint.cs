using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static VisualizerSystem.ProblemSolvedEvent;
using static TreeScripts.ControlTree;

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


        public void Initialize(Vector3 pos, Vector3 up, float animationTime, Genre genre)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            GetComponent<MeshRenderer>().material.SetColor("_Color", GENRE_TO_COLOR[(int)genre]);
            this.origin = camera.transform.position + camera.transform.forward;
            this.control = pos + up * 0.5f;
            this.target = pos;
            isStart = true;
            progress = 0f;
            DOTween.To(() => progress, (val) => {
                progress = val;
            }, 1f, 0.4f * animationTime).SetEase(Ease.InCirc)
            .OnComplete(() => {
                Destroy(gameObject);
            });
        }

        void Update()
        {
            if(isStart)
            {
                float t = progress;
                float s = 1f - progress;
                Vector3 pos = origin * s * s + 2f * control * t * s + target * t * t;
                transform.position = pos;
                transform.localScale = new Vector3(0.4f, 0.4f, 0.4f) * (1f - progress);
            }
        }
    }
}
