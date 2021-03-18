using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree;

namespace Tree {
  public class TreeGenerator : MonoBehaviour
  {
    [SerializeField]
    GameObject tree;
    [SerializeField]
    GameObject leave;
    [SerializeField]
    Color leaveBaseColor;
    [SerializeField]
    float baseColorRate = 0.5f;
    [SerializeField]
    Transform treeParent;
    [SerializeField]
    int num, segmentNum, branchNum;
    [SerializeField]
    float radius, branchLength, stemRadius;
    UserManager userManager;
    void Start()
    {
      userManager = GetComponent<UserManager>();
      StartCoroutine("MakeTree");

      // 葉の色
      for(int i=0;i<Points.GENRE_TO_COLOR.Length;i++) {
        Points.GENRE_TO_COLOR[i] = Points.GENRE_TO_COLOR[i] * 0.95f + new Color(1f, 1f, 1f) * 0.05f;
        Points.GENRE_TO_COLOR[i] = Points.GENRE_TO_COLOR[i] * baseColorRate + leaveBaseColor * (1f - baseColorRate);
        // Points.GENRE_TO_COLOR[i] = Mathf.Pow(new Color(1f, 1f, 1f) - Points.GENRE_TO_COLOR[i], 2.0f);
      }
    }

    IEnumerator MakeTree() {
      List<User> users = new List<User>();
      TreeMesh treeMesh = new TreeMesh(segmentNum, branchNum, branchLength, stemRadius);
      treeMesh.BuildMesh();
      if(treeParent.childCount == 0)
      {
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
          yield return new WaitForSeconds(.01f);
        }
      } else {
        
        num = treeParent.childCount;
        float phi = 0f;
        for(int i=0;i<num;i++) {
          GameObject t = treeParent.GetChild(i).gameObject;
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
          yield return new WaitForSeconds(.01f);
        }
      }

      userManager.SetTrees(users);
    }


    void Update()
    {
      
    }
  }

}
