using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlanter : MonoBehaviour
{
  [SerializeField]
  GameObject[] trees;
  [SerializeField]
  Transform treeParent;
  [SerializeField]
  GameObject fakeTree;
  [SerializeField]
  Transform fakeTreeParent;
  [SerializeField]
  Transform pointer;

  void Update()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit_info = new RaycastHit();
    float max_distance = 100f;
    int layerMask = 1 << LayerMask.NameToLayer("Planet");

    bool is_hit = Physics.Raycast(ray, out hit_info, max_distance, layerMask);

    if (is_hit)
    {
      Vector3 dir = hit_info.point.normalized;
      float phi = Mathf.Atan2(dir.z, dir.x);
      float theta = Mathf.Acos(dir.y);
      Quaternion quat = Quaternion.AngleAxis(-phi * Mathf.Rad2Deg, new Vector3(0f, 1f, 0f)) *
              Quaternion.AngleAxis(-theta * Mathf.Rad2Deg, new Vector3(0f, 0f, 1f)) *
              Quaternion.AngleAxis(Random.Range(0f, 360f), new Vector3(0f, 1f, 0f));

      if (Input.GetMouseButtonDown(0))
      {
        Transform treeInstance = Instantiate(trees[Random.Range(0, trees.Length)], treeParent).transform;
        treeInstance.position = hit_info.point;
        treeInstance.rotation = quat;
        Transform fakeTreeInstance = Instantiate(fakeTree, fakeTreeParent).transform;
        fakeTreeInstance.position = hit_info.point;
        fakeTreeInstance.rotation = quat;
        Debug.Log(treeParent.childCount);
      }
      else
      {
        pointer.position = hit_info.point;
        pointer.rotation = quat;
      }

    }

    if (Input.GetKeyDown(KeyCode.Z))
    {
      Destroy(treeParent.GetChild(treeParent.childCount - 1).gameObject);
      Destroy(fakeTreeParent.GetChild(fakeTreeParent.childCount - 1).gameObject);
      Debug.Log(treeParent.childCount);
    }
  }
}
