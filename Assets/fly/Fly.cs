using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    float seed;
    void Start() {
        MaterialPropertyBlock prop = new MaterialPropertyBlock();
        prop.SetColor("_Color", new Color(Random.value, Random.value, Random.value));
        gameObject.GetComponent<SpriteRenderer>().SetPropertyBlock(prop);
        seed = Random.Range(0f, Mathf.PI * 2f);
    }

    void FixedUpdate()
    {
        float scale = 10f;
        float t = Time.time + seed;
        float x = Mathf.Sin(t * 0.1f * scale) * 0.3f + Mathf.Sin(t * 0.3f * scale) * 0.1f + Mathf.Sin(t * 0.2f * scale) * 0.6f;
        float y = Mathf.Sin(t * 0.1f * scale) * 0.6f + Mathf.Sin(t * 0.3f * scale) * 0.25f + Mathf.Sin(t * 0.2f * scale) * 0.15f;
        float z = Mathf.Sin(t * 0.1f * scale) * 0.3f + Mathf.Sin(t * 0.3f * scale) * 0.5f + Mathf.Sin(t * 0.2f * scale) * 0.2f;
        transform.position = transform.position + new Vector3(x, y, z) / 120f;
    }
    
}
