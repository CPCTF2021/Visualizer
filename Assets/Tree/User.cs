using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tree;

public class User : MonoBehaviour
{
  [SerializeField]
  UserIcon userIcon;
  ControlTree controlTree;
  Points points;
  string name, id;
  Texture icon;
  void Start()
  {
    controlTree = GetComponent<ControlTree>();
  }

  public void SetUser(string name, string id, Texture icon, Points points)
  {
    this.name = name;
    this.id = id;
    this.icon = icon;
    this.points = points;
    
    controlTree.points = points;
    controlTree.SetActive(true);
    // 10000fは最大ポイント TODO
    controlTree.AnimationTree(points.sum / 10000f * 0.7f + 0.3f);
    userIcon.gameObject.SetActive(true);
    userIcon.SetIcon(icon);
  }

  public void AddPoint(int genre, int point)
  {
    points.Add(genre, point);
    // 10000fは最大ポイント TODO
    controlTree.AnimationTree(points.sum / 10000f * 0.7f + 0.3f);
  }

  public Vector3 GetPosition()
  {
    return transform.position + transform.rotation * Vector3.up * 0.7f;
    // return transform.position * 1.1f;
  }
}
