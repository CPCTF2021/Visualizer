using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField]
    GameObject flyObject;
    void Start() {
        for(int i=0;i<1000;i++) {
            GameObject fly = Instantiate(flyObject, transform);
            fly.transform.position = Random.insideUnitSphere * 10f;
            MaterialPropertyBlock prop = new MaterialPropertyBlock();
            prop.SetColor("_Color", new Color(Random.value, Random.value, Random.value));
            fly.GetComponent<SpriteRenderer>().SetPropertyBlock(prop);
        }
    }
}
