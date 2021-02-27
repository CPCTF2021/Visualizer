using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserController : MonoBehaviour
{
    ControlTree controlTree;
    Points points = new Points();
    string name, id;
    void Start() {
        controlTree = GetComponent<ControlTree>();
        controlTree.progress = 0f;
    }

    public void SetUser(string name, string id, Points points) {
        this.name = name;
        this.id = id;
        this.points = points;
        int sum = points.Sum();
        // 10000fは最大ポイント
        controlTree.progress = sum / 10000f * 0.9f + 0.1f;
    }

    public void SetSolvedProblem(int genre, int point) {
        points.Add(genre, point);
        int sum = points.Sum();
        // 10000fは最大ポイント
        controlTree.progress = sum / 10000f * 0.9f + 0.1f;
    }
}
