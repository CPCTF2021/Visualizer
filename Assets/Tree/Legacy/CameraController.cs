using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 targetRotation, nowRotation;
    float targetScale, nowScale;
    void Start()
    {
        targetRotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        targetScale = Random.Range(1f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        nowRotation += (targetRotation - nowRotation) * 0.01f;
        nowScale += (targetScale - nowScale) * 0.01f;
        transform.rotation = Quaternion.Euler(nowRotation);
        transform.localScale = new Vector3(nowScale, nowScale, nowScale);
        if(Time.time % 2f < Time.deltaTime) {
            targetRotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            targetScale = Random.Range(3f, 6f);
        }
    }
}
