using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcLegacy : MonoBehaviour
{
    [SerializeField]
    int segmentNum = 30;
    [SerializeField]
    float width = 0.04f;

    void Start()
    {
        float delta = 0f;
        Vector3[] vertices = new Vector3[segmentNum * 5];
        for(int i=0;i<segmentNum;i++) {
            for(int j=0;j<5;j++) {
                vertices[i*5+j] = new Vector3(
                    Mathf.Cos(Mathf.PI / 5f * j + delta) * width,
                    i / (float)segmentNum,
                    Mathf.Sin(Mathf.PI / 5f * j + delta) * width
                );
                delta += Mathf.PI / 5f;
            }
        }
        int[] triangles = new int[(segmentNum - 1) * 10 * 3];
        for(int i=1;i<segmentNum;i++) {
            for(int j=0;j<5;j++) {
                triangles[((i - 1) * 10 + j) * 3    ] = i * 5 + j;
                triangles[((i - 1) * 10 + j) * 3 + 1] = i * 5 + (j + 1) % 5;
                triangles[((i - 1) * 10 + j) * 3 + 2] = (i - 1) * 5 + (j + 3) % 5;
            }
            for(int j=0;j<5;j++) {
                triangles[((i - 1) * 10 + j + 5) * 3    ] = (i - 1) * 5 + j;
                triangles[((i - 1) * 10 + j + 5) * 3 + 1] = i * 5 + (j + 3) % 5;
                triangles[((i - 1) * 10 + j + 5) * 3 + 2] = (i - 1) * 5 + (j + 1) % 5;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
