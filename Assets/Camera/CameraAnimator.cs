﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CameraScripts
{

    public class CameraAnimator : MonoBehaviour
    {
        Camera cam;
        Transform camTransform;

        float theta, phi;
        float targetTheta, targetPhi;
        float targetHeight = 1f;
        Vector3 target;
        Vector3 normal;
        Vector3 binormal;
        float progress;

        Sequence sequence;
        [SerializeField]
        float radius = 5f;
        [SerializeField]
        float treeAround = 1f;

        float treeAroundRadius;
        RankingMove rankingMove;
        PostProcessing postProcessing;

        void Start()
        {
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
            camTransform = cam.transform;
            progress = theta = phi = targetTheta = targetPhi = 0.0f;
            target = normal = binormal = Vector3.zero;
            rankingMove = GameObject.Find("RankingPanel").GetComponent<RankingMove>();
            postProcessing = GetComponent<PostProcessing>();
        }

        void PositionCalculate()
        {
            float t = Mathf.Lerp(Mathf.Sin(theta) * Mathf.PI * 0.4f + Mathf.PI * 0.5f, targetTheta, progress);
            float p = Mathf.Lerp(phi, targetPhi, progress);

            camTransform.position = new Vector3(
                Mathf.Sin(t) * Mathf.Cos(p),
                Mathf.Cos(t),
                Mathf.Sin(t) * Mathf.Sin(p)
            ) * Mathf.Lerp(radius * 3.0f, targetHeight
            , progress) * 0.5f
                + Vector3.Lerp(Vector3.zero,
                normal * Mathf.Cos(phi * 5f) * treeAround + binormal * Mathf.Sin(phi * 5f) * treeAround,
                progress) * treeAroundRadius;

            theta = (theta + Mathf.PI * 0.0017f) % (Mathf.PI * 2f);
            phi = (phi + Mathf.PI * 0.001f) % (Mathf.PI * 2f);
            camTransform.LookAt(Vector3.Lerp(Vector3.zero, target.normalized * targetHeight * 0.5f, progress), Vector3.Lerp(Vector3.up, target, progress));
            camTransform.position = camTransform.position + Vector3.Lerp(radius * 0.5f * camTransform.right, Vector3.zero, progress);
        }

        void Update()
        {
            PositionCalculate();
            postProcessing.SetProgress(progress);
        }

        public Sequence MoveToTarget(float t)
        {
            if (sequence != null) sequence.Kill(false);
            treeAroundRadius = 1.5f;
            sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => progress, (val) =>
            {
                progress = val;
            }, 1f, t));
            rankingMove.RightMove(t);

            return sequence;
        }

        public void ChangeTarget(Vector3 pos)
        {
            target = pos;
            float radius = pos.magnitude;
            targetHeight = radius * 2.0f + 0.1f * treeAround;
            Vector3 dir = pos / radius;
            targetTheta = Mathf.Acos(dir.y);
            if (pos.z == 0) targetPhi = 0f;
            else targetPhi = Mathf.Atan2(pos.z, pos.x);
            normal = Vector3.Cross(dir, Vector3.up).normalized;
            binormal = Vector3.Cross(dir, normal).normalized;
            PositionCalculate();
        }
        
        public void LeaveFromTarget(float t)
        {
            sequence.Append(DOTween.To(() => progress, (val) =>
            {
                progress = val;
            }, 0f, t));
            rankingMove.LeftMove(t);
        }
    }
}
