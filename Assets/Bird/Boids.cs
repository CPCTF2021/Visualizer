using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
  Vector3 direction;
  [SerializeField]
  float maxSpeed = 3f;
  [SerializeField]
  float radius = 5f;

  Vector3 delta;
  void Start()
  {
    direction = Vector3.Cross(transform.position.normalized, Vector3.up);
    transform.position = transform.position / transform.position.magnitude * radius;
    delta = Vector3.zero;
  }

  // Update is called once per frame
  void Update()
  {
    direction += delta;
    direction = direction / direction.magnitude * maxSpeed;
    delta = Vector3.zero;
    if(direction) direction = Vector3.zero;
    transform.position = transform.position + direction * Time.deltaTime;
    transform.position = transform.position / transform.position.magnitude * radius;
  }

  void OnTriggerStay(Collider col)
  {
    if (col.gameObject.tag == "Area")
    {
      Boids boids = col.transform.parent.gameObject.GetComponent<Boids>();
      Vector3 dir = boids.transform.position - transform.position;
      float dist = dir.magnitude;
      if (dist < 3f)
      {
        float val = 2f;
        delta -= new Vector3(val / dir.x, val / dir.y, val / dir.z);
      }
      else
      {
        delta += dir / dist * 3f;
      }
    }
  }
}
