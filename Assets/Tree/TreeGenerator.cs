using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeScripts;
using UserScripts;

namespace TreeScripts {
  public class TreeGenerator : MonoBehaviour
  {
    [SerializeField]
    GameObject tree;
    [SerializeField]
    GameObject leave;
    [SerializeField]
    Transform treeParent;
    [SerializeField]
    int num, segmentNum, branchNum;
    [SerializeField]
    float radius, branchLength, stemRadius;

    public void MakeTree() {
      List<User> users = new List<User>();
      TreeMesh treeMesh = new TreeMesh(segmentNum, branchNum, branchLength, stemRadius);
      treeMesh.BuildMesh();
      if(treeParent.childCount == 0)
      {
        // 自動配置
        float phi = 0f;
        for(int i=0;i<num;i++) {
          GameObject t = Instantiate(tree, treeParent);
          users.Add(t.GetComponent<User>());
          treeMesh.SetMesh(t.GetComponent<ControlTree>(), leave);
          float h = 2f * i / (num - 1f) - 1f;
          float theta = Mathf.Acos(h);
          if(Mathf.Abs(h) != 1f) phi = phi + 3.6f / Mathf.Sqrt(num * (1f - h * h));
          Vector3 dir = new Vector3(
            Mathf.Sin(theta) * Mathf.Cos(phi),
            Mathf.Cos(theta),
            Mathf.Sin(theta) * Mathf.Sin(phi)
          ) * radius;
          t.transform.position = dir;
          Quaternion quat = Quaternion.AngleAxis(-phi * Mathf.Rad2Deg, new Vector3(0f, 1f, 0f)) *
                          Quaternion.AngleAxis(-theta * Mathf.Rad2Deg, new Vector3(0f, 0f, 1f)) *
                          Quaternion.AngleAxis(Random.Range(0f, 360f), new Vector3(0f, 1f, 0f));
          // Quaternion quat = Quaternion.AngleAxis(-theta / Mathf.PI / 2f * 360f, new Vector3(0f, 0f, 1f));
          t.transform.rotation = quat;
        }
      } else {
        // すでに配置されてるのをActiveに
        num = treeParent.childCount;
        for(int i=0;i<num;i++) {
          GameObject t = treeParent.GetChild(i).gameObject;
          users.Add(t.GetComponent<User>());
          treeMesh.SetMesh(t.GetComponent<ControlTree>(), leave);
        }
      }
    }
  }

}
