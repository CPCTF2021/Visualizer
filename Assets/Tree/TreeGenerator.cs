using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject tree;
    [SerializeField]
    Transform treeParent;
    [SerializeField]
    int num;
    [SerializeField]
    float radius;
    public List<UserController> userControllers;
    void Start()
    {
        userControllers = new List<UserController>();
        float phi = 0f;
        for(int i=1;i<num;i++) {
            GameObject t = Instantiate(tree, treeParent);
            userControllers.Add(t.GetComponent<UserController>());
            float h = 2f * i / (num - 1f) - 1f;
            float theta = Mathf.Acos(h);
            phi = phi + 3.6f / Mathf.Sqrt(num * (1f - h * h));
            Vector3 dir = new Vector3(
                Mathf.Sin(theta) * Mathf.Cos(phi),
                Mathf.Cos(theta),
                Mathf.Sin(theta) * Mathf.Sin(phi)
            ) * radius;
            t.transform.position = dir;
            Quaternion quat = Quaternion.AngleAxis(-phi * Mathf.Rad2Deg, new Vector3(0f, 1f, 0f)) *
                            Quaternion.AngleAxis(-theta * Mathf.Rad2Deg, new Vector3(0f, 0f, 1f));
            // Quaternion quat = Quaternion.AngleAxis(-theta / Mathf.PI / 2f * 360f, new Vector3(0f, 0f, 1f));
            t.transform.rotation = quat;
        }

        for(int i=0;i<1000;i++) {
            Invoke("GetPoint", i * 0.1f);
        }
    }

    void GetPoint() {
        int index = Random.Range(0, userControllers.Count);
        userControllers[index].SetSolvedProblem(Random.Range(0, 10), 1000);
    }

    void Update()
    {
        
    }
}
