using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class Leaves : MonoBehaviour
{
    [SerializeField]
    float dist = 5f;
    GameObject sphere, plane;
    void Start() {
        sphere = transform.GetChild(0).gameObject;
        plane = transform.GetChild(1).gameObject;
    }
	void Update () {
		Vector3 p = Camera.main.transform.position;
        Vector3 pos = transform.position;

        bool flag = Vector3.Distance(p, pos) > dist;

        sphere.SetActive(!flag);
        plane.SetActive(flag);
		if(flag) transform.LookAt(-(p - transform.position) + transform.position);
	}
}
