using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreeScripts {

  public class TreeMesh {

    protected class Coordinate {
      public Vector3 normal;
      public Vector3 binormal;
      public Vector3 front;
      public Coordinate() {
        normal = new Vector3(1f, 0f, 0f);
        binormal = new Vector3(0f, 0f, 1f);
        front = new Vector3(0f, 1f, 0f);
      }

      public Coordinate(Vector3 normal, Vector3 binormal, Vector3 front) {
        this.normal = normal;
        this.binormal = binormal;
        this.front = front;
      }

      public Coordinate Rotate(float branchDelta) {
        Vector3 prevDir = front;
        front = Random.Range(-branchDelta, branchDelta) * normal + Random.Range(-branchDelta, branchDelta) * binormal + front;
        front = front.normalized;
        Quaternion rotation = Quaternion.FromToRotation(prevDir, front);
        normal = rotation * normal;
        binormal = rotation * binormal;
        return this;
      }

      public Coordinate Copy() {
          return new Coordinate(this.normal, this.binormal, this.front);
      }
    }

    Mesh mesh;
    int segmentNum, branchNum;
    float branchLength, radius;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv2 = new List<Vector2>();
    List<Vector2> uv3 = new List<Vector2>();

    public List<Vector3> leavePosition = new List<Vector3>();
    List<float> leaveProgress = new List<float>();
    public TreeMesh(int segmentNum, int branchNum, float branchLength, float radius) {
      this.segmentNum = segmentNum;
      this.branchNum = branchNum;
      this.branchLength = branchLength;
      this.radius = radius;
    }

    protected void BuildBranch(int branchIndex, Vector3 offset, Coordinate _coord) {
      int segmentNum = this.segmentNum;
      if(branchIndex >= branchNum) return;
      Coordinate prevCoord = _coord.Copy();
      Coordinate coord = _coord;
      float delta = 0f;
      Vector3 offset2 = offset + coord.front / (float)(branchNum * segmentNum) * branchLength;
      int indexOffset = triangles.Count;
      if(branchIndex >= branchNum - 3) {
        leavePosition.Add(offset + coord.front / (float)branchNum * 0.5f * branchLength);
        leaveProgress.Add(branchIndex / (float)branchNum);
      }
      for(int i=0;i<segmentNum-1;i++) {
        for(int j=0;j<3;j++) {
          float rad1 = delta + j / 3f * Mathf.PI * 2f;
          float rad2 = delta + (j+1) / 3f * Mathf.PI * 2f;
          Vector3 downRingNormal1 = prevCoord.normal * Mathf.Cos(rad1) + prevCoord.binormal * Mathf.Sin(rad1);
          Vector3 downRingNormal2 = prevCoord.normal * Mathf.Cos(rad2) + prevCoord.binormal * Mathf.Sin(rad2);
          Vector3 downRing1 = downRingNormal1 * radius + offset;
          Vector3 downRing2 = downRingNormal2 * radius + offset;
          Vector3 upRingNormal1 = coord.normal * Mathf.Cos(rad1) + coord.binormal * Mathf.Sin(rad1);
          Vector3 upRingNormal2 = coord.normal * Mathf.Cos(rad2) + coord.binormal * Mathf.Sin(rad2);
          Vector3 upRing1 = upRingNormal1 * radius + offset2;
          Vector3 upRing2 = upRingNormal2 * radius + offset2;
          vertices.Add(downRing1);vertices.Add(upRing1);vertices.Add(downRing2);
          vertices.Add(upRing1);vertices.Add(upRing2);vertices.Add(downRing2);
          triangles.Add(indexOffset + i * 18 + j * 6 + 0);
          triangles.Add(indexOffset + i * 18 + j * 6 + 1);
          triangles.Add(indexOffset + i * 18 + j * 6 + 2);
          triangles.Add(indexOffset + i * 18 + j * 6 + 3);
          triangles.Add(indexOffset + i * 18 + j * 6 + 4);
          triangles.Add(indexOffset + i * 18 + j * 6 + 5);
          float downProgress = 1f - (branchIndex + i / (float)(segmentNum - 1)) / (float)branchNum;
          float upProgress = 1f - (branchIndex + (i + 1) / (float)(segmentNum - 1)) / (float)branchNum;
          uv2.Add(new Vector2(downProgress, downRingNormal1.x));uv3.Add(new Vector2(downRingNormal1.y, downRingNormal1.z));
          uv2.Add(new Vector2(upProgress, upRingNormal1.x));uv3.Add(new Vector2(upRingNormal1.y, upRingNormal1.z));
          uv2.Add(new Vector2(downProgress, downRingNormal2.x));uv3.Add(new Vector2(downRingNormal2.y, downRingNormal2.z));
          uv2.Add(new Vector2(upProgress, upRingNormal1.x));uv3.Add(new Vector2(upRingNormal1.y, upRingNormal1.z));
          uv2.Add(new Vector2(upProgress, upRingNormal2.x));uv3.Add(new Vector2(upRingNormal2.y, upRingNormal2.z));
          uv2.Add(new Vector2(downProgress, downRingNormal2.x));uv3.Add(new Vector2(downRingNormal2.y, downRingNormal2.z));
        }
        if(i == segmentNum - 1) break;
        offset = offset2;
        offset2 += coord.front / (float)(branchNum * segmentNum) * branchLength;
        // delta += Mathf.PI / 3f;
        prevCoord = coord.Copy();
        coord.Rotate(0f * (branchIndex / (float)branchNum));
      }
      BuildBranch(branchIndex + Random.Range(2, 5), offset, prevCoord.Copy().Rotate(10f * (branchIndex / (float)branchNum)));
      BuildBranch(branchIndex + 1, offset, prevCoord);
      BuildBranch(branchIndex + Random.Range(2, 5), offset, prevCoord.Copy().Rotate(10f * (branchIndex / (float)branchNum)));
    }

    public void BuildMesh() {
      BuildBranch(1, Vector3.zero, new Coordinate());
      mesh = new Mesh();
      mesh.SetVertices(vertices.ToArray());
      mesh.SetTriangles(triangles.ToArray(), 0);
      // for(int i=0;i<vertices.Count;i++) Debug.Log(vertices[i]);
      mesh.RecalculateNormals();
      mesh.SetUVs(1, uv2.ToArray());
      mesh.SetUVs(2, uv3.ToArray());
    }

    public void SetMesh(ControlTree controlTree, GameObject leave) {
      controlTree.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
      List<Transform> leaves = new List<Transform>();
      for(int i=0;i<leavePosition.Count;i++) {
        Transform leaveTransform = MonoBehaviour.Instantiate(leave, controlTree.transform).transform;
        leaveTransform.localPosition = leavePosition[i];
        leaveTransform.localScale = Vector3.zero;
        leaves.Add(leaveTransform);
      }
      controlTree.radius = radius;
      controlTree.branchNum = branchNum;
      controlTree.leaveList = leaves;
      controlTree.leaveProgress = leaveProgress;
      controlTree.ResetTree();
    }
  }
}