using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAnimator : MonoBehaviour
{
    Camera cam;
    Transform camTransform;

    float theta, phi;
    float targetTheta, targetPhi;
    Vector3 target;
    Vector3 normal;
    Vector3 binormal;
    float progress;

    Sequence sequence;
    [SerializeField]
    float radius = 5f;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        camTransform = cam.transform;
        progress = theta = phi = targetTheta = targetPhi = 0.0f;
        target = normal = binormal = Vector3.zero;
    }

    void Update()
    {
        float t = Mathf.Lerp(theta, targetTheta, progress);
        float p = Mathf.Lerp(phi, targetPhi, progress);

        camTransform.position = new Vector3(
            Mathf.Sin(t) * Mathf.Cos(p),
            Mathf.Cos(t),
            Mathf.Sin(t) * Mathf.Sin(p)
        ) * Mathf.Lerp(radius * 3.0f, radius + 1f, progress) * 0.5f
            + Vector3.Lerp(Vector3.zero, 
            normal * Mathf.Cos(phi * 5f) + binormal * Mathf.Sin(phi * 5f),
            progress) * 1.5f;

        theta = (theta + Mathf.PI * 0.001f) % (Mathf.PI * 2f);
        phi = (phi + Mathf.PI * 0.001f) % (Mathf.PI * 2f);
        camTransform.LookAt(Vector3.Lerp(Vector3.zero, target, progress), Vector3.Lerp(Vector3.up, target, progress));
    }

    public void SetTarget(Vector3 pos)
    {
        target = pos;
        float radius = pos.magnitude;
        Vector3 dir = pos / radius;
        targetTheta = Mathf.Acos(dir.y);
        if(pos.z == 0) targetPhi = 0f;
        else targetPhi = Mathf.Atan2(pos.z, pos.x);
        if(sequence != null) sequence.Kill(false);
        sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => progress, (val) => {
            progress = val;
        }, 1f, 1f));
        sequence.AppendInterval(1f);
        sequence.Append(DOTween.To(() => progress, (val) => {
            progress = val;
        }, 0f, 1f));

        normal = Vector3.Cross(dir, Vector3.up).normalized;
        binormal = Vector3.Cross(dir, normal).normalized;
    }
}
