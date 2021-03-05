using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tree {
    public class ControlTree : MonoBehaviour
    {
        [Range(0,1)]
        public float progress = 0f;
        float prevProgress = 0f;

        Material material;
        public List<Transform> leaveList;
        public List<float> leaveProgress;
        public int branchNum;
        public float radius;
        public Points points;
        bool isGrow = false;

        public void SetActive(bool flag) {
            isGrow = flag;
        }
        void GrowTree() {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            progress = Mathf.Max(Mathf.Min(progress, 1f), 0f);
            props.SetFloat("_Radius", radius);
            props.SetFloat("_Progress", progress);
            GetComponent<MeshRenderer>().SetPropertyBlock(props);
            // GetComponent<Renderer>().material.SetFloat("_Progress", nowProgress);
            transform.localScale = new Vector3(progress, progress, progress);
            int index = 0;
            for(int i=0;i<leaveList.Count;i++) {
                float scale = radius * 200f * Mathf.Max(Mathf.Min((progress - leaveProgress[i]) * (float)branchNum * 0.3f, 1f), 0f);
                
                leaveList[i].localScale = new Vector3(scale, scale, scale);
                MaterialPropertyBlock props2 = new MaterialPropertyBlock();
                while(index + 1 < 10 && points.cumulativeParcentage[index + 1] < i / (float)leaveList.Count) {
                    index ++;
                }
                props2.SetColor("_Color", Points.GENRE_TO_COLOR[index]);
                leaveList[i].gameObject.GetComponent<MeshRenderer>().SetPropertyBlock(props2);
            }
            prevProgress = progress;

        }
        void Start()
        {
            material = GetComponent<Renderer>().material;
            GrowTree();
        }
        void Update()
        {
            if(!isGrow) return;
            progress = Mathf.Min(progress, 1f);
            if(progress != prevProgress) {
                GrowTree();
            }
        }
    }

}
