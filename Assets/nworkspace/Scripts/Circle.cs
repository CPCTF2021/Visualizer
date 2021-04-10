using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    float speed = 1.5f;
    float radious = 20.0f;
    float theta = Mathf.PI / 6.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = circleMove(this.transform.position, 0.0f, radious, radious);
    }

    Vector3 circleMove(Vector3 pos, float theta, float aradious, float bradious)
    {
        Vector3 res = pos;
        res.x = aradious * Mathf.Cos(Time.time * speed);
        res.z = bradious * Mathf.Sin(Time.time * speed);
        res.y = res.x * Mathf.Sin(theta);
        res.x = res.x * Mathf.Cos(theta);
        return res;
    }
}
