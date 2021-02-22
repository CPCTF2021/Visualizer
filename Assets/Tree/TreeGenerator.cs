using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject tree;
    [SerializeField]
    int width, height;
    void Start()
    {
        for(int x=-width/2;x < width/2;x++) {
            for(int y=-height/2;y < height/2;y++) {
                GameObject t = Instantiate(tree, transform.position + new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        
    }
}
