using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tree {
    public class ControlTree : MonoBehaviour
    {
        [Range(0,1)]
        public float progress = 0f;
        float prevProgress = 0f;
        float nowProgress;

        Material material;
        public List<Transform> leaveList;
        public List<float> leaveProgress;
        public int branchNum;
        public float radius;

        void GrowTree() {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            props.SetFloat("_Radius", radius);
            props.SetFloat("_Progress", nowProgress);
            GetComponent<MeshRenderer>().SetPropertyBlock(props);
            // GetComponent<Renderer>().material.SetFloat("_Progress", nowProgress);
            transform.localScale = new Vector3(nowProgress, nowProgress, nowProgress);
            for(int i=0;i<leaveList.Count;i++) {
                float scale = radius * 200f * Mathf.Max(Mathf.Min((nowProgress - leaveProgress[i]) * (float)branchNum, 1f), 0f);
                
                leaveList[i].localScale = new Vector3(scale, scale, scale);
            }
            prevProgress = progress;

        }
        void Start()
        {
            material = GetComponent<Renderer>().material;
        }
        void Update()
        {
            nowProgress += (progress - nowProgress) * 0.04f;
            // if(progress != prevProgress) {
                GrowTree();
            // }
        }
    }

}
