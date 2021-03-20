using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalGravity : MonoBehaviour
{
    public GameObject star;
    Rigidbody rigidbody;
    Rigidbody starRigidbody;

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        starRigidbody = star.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 dir = star.transform.position - this.transform.position;
        float dist = dir.magnitude;
        float force = Const.GRAVITY * starRigidbody.mass * rigidbody.mass / (dist * dist);
        rigidbody.AddForce(force * dir.normalized, ForceMode.Force);
    }
}
