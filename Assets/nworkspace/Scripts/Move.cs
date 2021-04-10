using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    float power = 5.0f;
    float starMass;
    Vector3 starPos;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        starMass = GameObject.Find("Star").GetComponent<Rigidbody>().mass;
        starPos = GameObject.Find("Star").transform.position;
        Vector3 rpos = (starPos - this.transform.position);
        float v = Mathf.Sqrt(Const.GRAVITY * starMass / rpos.magnitude);
        rigidbody.velocity = new Vector3(0.0f, 0.0f, v);
        Debug.Log(v);
    }
}
