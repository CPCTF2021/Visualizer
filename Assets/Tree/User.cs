using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tree;

public class User : MonoBehaviour
{
    ControlTree controlTree;
    Points points;
    string name, id;
    Sprite icon;
    void Start() {
        controlTree = GetComponent<ControlTree>();
    }

    public void SetUser(string name, string id, Sprite icon, Points points) {
        this.name = name;
        this.id = id;
        this.icon = icon;
        this.points = points;
        controlTree.points = points;
        controlTree.SetActive(true);
        // 10000fは最大ポイント TODO
        controlTree.progress = points.sum / 10000f * 0.7f + 0.3f;
    }

    public void AddPoint(int genre, int point) {
        points.Add(genre, point);
        // 10000fは最大ポイント TODO
        controlTree.progress = points.sum / 10000f * 0.7f + 0.3f;
    }
}
