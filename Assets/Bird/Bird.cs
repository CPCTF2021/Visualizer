using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bird
{
  public class Bird : MonoBehaviour
  {
    [SerializeField]
    float radius;
    Vector3 direction;
    Vector3 binormal;
    Vector3 normal;
    [SerializeField]
    float seed = 0.1f;
    void Start()
    {
      direction = new Vector3(1f, 0f, 0f);
      binormal = new Vector3(0f, 0f, -1f);
      normal = new Vector3(0f, 1f, 0f);
      // int count = transform.childCount;
      // for (int i = 0; i < count; i++)
      // {
      //   Transform bird = transform.GetChild(i);
      //   float rate = (radius / bird.localPosition.magnitude);
      //   bird.localPosition = bird.localPosition * rate;
      //   bird.localScale = bird.localScale * rate;
      // }

      Quaternion quat = Quaternion.AngleAxis(seed, binormal);
      transform.rotation = quat * transform.rotation;
      direction = quat * direction;
      normal = quat * normal;
      quat = Quaternion.AngleAxis(seed, normal);
      transform.rotation = quat * transform.rotation;
      direction = quat * direction;
      binormal = quat * binormal;
    }

    void Update()
    {
      Quaternion quat = Quaternion.AngleAxis(0.15f, binormal);
      transform.rotation = quat * transform.rotation;
      direction = quat * direction;
      normal = quat * normal;
      quat = Quaternion.AngleAxis(Mathf.Sin(Time.time * 0.5f) * 0.2f, normal);
      transform.rotation = quat * transform.rotation;
      direction = quat * direction;
      binormal = quat * binormal;
    }
  }
}
