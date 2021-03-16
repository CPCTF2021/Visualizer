using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserIcon : MonoBehaviour
{
    public void SetIcon(Texture texture) {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }

    void Update() {
        transform.Rotate(0f, Mathf.PI * 0.1f, 0f);
    }
}
