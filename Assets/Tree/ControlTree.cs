using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTree : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    float progress = 0f;
    float prevProgress = 0f;

    Material material;
    ProcLegacy tree;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        tree = GetComponent<ProcLegacy>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.SetFloat("_Progress", progress);
        transform.localScale = new Vector3(progress, progress, progress);
        if(progress != prevProgress) {
            for(int i=0;i<tree.leavesList.Count;i++) {
                float scale = tree.radius * 200f * Mathf.Max(Mathf.Min((progress - tree.leavesProgress[i]) * (float)tree.branchNum, 1f), 0f);
                
                tree.leavesList[i].localScale = new Vector3(scale, scale, scale);
            }
            prevProgress = progress;
        }
    }
}
