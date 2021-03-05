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
        float x = radious * Mathf.Cos(Time.time * speed);
        float z = radious * Mathf.Sin(Time.time * speed);
        float y = x * Mathf.Sin(theta);
        x = x * Mathf.Cos(theta);
        this.transform.position = new Vector3(x, y, z);
    }
}
