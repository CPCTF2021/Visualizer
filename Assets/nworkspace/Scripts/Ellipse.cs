using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellipse : MonoBehaviour
{
    const float radius1 = 5.0f;
    const float radius2 = 8.0f;
    public float time = 0.0f;

    void Update()
    {
        time += Time.deltaTime;
        Vector3 tmpPos = this.transform.position;
        tmpPos.x = radius1 * Mathf.Cos(2 * Mathf.PI * time);
        tmpPos.y = radius2 * Mathf.Sin(2 * Mathf.PI * time);
        this.transform.position = tmpPos;
    }
}
