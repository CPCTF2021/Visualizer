using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlTree : MonoBehaviour
{
    [Range(0,1)]
    public float progress = 0f;
    float prevProgress = 0f;

    Material material;
    ProcLegacy tree;

    void GrowTree() {
        transform.localScale = new Vector3(progress, progress, progress);
        for(int i=0;i<tree.leavesList.Count;i++) {
            float scale = tree.radius * 200f * Mathf.Max(Mathf.Min((progress - tree.leavesProgress[i]) * (float)tree.branchNum, 1f), 0f);
            
            tree.leavesList[i].localScale = new Vector3(scale, scale, scale);
        }
        prevProgress = progress;
    }
    void Start()
    {
        material = GetComponent<Renderer>().material;
        tree = GetComponent<ProcLegacy>();
        GrowTree();
    }
    void Update()
    {
        GetComponent<Renderer>().material.SetFloat("_Progress", progress);
        if(progress != prevProgress) {
            GrowTree();
        }
    }
}
