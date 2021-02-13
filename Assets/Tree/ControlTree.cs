using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTree : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    float progress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.SetFloat("_Progress", progress);
        transform.localScale = new Vector3(progress, progress, progress);
    }
}
