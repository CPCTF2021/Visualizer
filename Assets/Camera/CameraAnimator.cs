using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tween;

public class CameraAnimator : MonoBehaviour
{
    Camera cam;
    Transform camTransform;

    float theta, phi;
    float targetTheta, targetPhi;
    float progress;
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        camTransform = cam.transform;
        progress = theta = phi = targetTheta = targetPhi = 0.0f;
    }

    void Update()
    {
        float t = Mathf.Lerp(theta, targetTheta, progress);
        float p = Mathf.Lerp(phi, targetPhi, progress);
        camTransform.position = new Vector3(
            Mathf.Sin(theta) * Mathf.Sin(phi),
            Mathf.Cos(theta),
            Mathf.Sin(theta) * Mathf.Cos(phi)
        ) * 6f;

        theta += Mathf.PI * 0.001f;
        phi += Mathf.PI * 0.001f;
        camTransform.LookAt(Vector3.zero);
    }

    public void SetTarget(Vector3 pos)
    {
        float radius = pos.magnitude;
        Vector3 dir = pos / radius;
        targetTheta = Mathf.Acos(dir.y);
        targetPhi = Mathf.Acos(dir.z / Mathf.Sin(theta));

    }
}
