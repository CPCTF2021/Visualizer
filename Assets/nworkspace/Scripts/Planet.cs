using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject star;
    [SerializeField] float initVelZ;
    const float G = 6.67E-5f;
    float massOfPlanet;
    float massOfStar;
    Rigidbody rigidbody;

    void Start()
    {
        Vector3 initVel = new Vector3(0f, 0f, initVelZ);
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = initVel;
        massOfStar = star.GetComponent<Rigidbody>().mass;
        massOfPlanet = rigidbody.mass;
    }

    void FixedUpdate()
    {
        Vector3 dir = star.transform.position - this.transform.position;
        float r = dir.magnitude;
        float f = G * massOfStar * massOfPlanet / (r * r);
        rigidbody.AddForce(f * dir.normalized, ForceMode.Force);
    }

}
