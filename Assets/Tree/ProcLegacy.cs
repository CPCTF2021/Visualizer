using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcLegacy : MonoBehaviour
{
    [SerializeField]
    int branchSegment = 10;
    public int branchNum = 5;
    [SerializeField]
    float branchDelta = 1f;
    public float radius = 0.04f;

    [SerializeField]
    GameObject leaves;

    public List<Transform> leavesList;
    public List<float> leavesProgress;

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

        public void Rotate(float branchDelta) {
            Vector3 prevDir = front;
            front = Random.Range(-branchDelta, branchDelta) * normal + Random.Range(-branchDelta, branchDelta) * binormal + front;
            front = front.normalized;
            Quaternion rotation = Quaternion.FromToRotation(prevDir, front);
            normal = rotation * normal;
            binormal = rotation * binormal;
        }

        public Coordinate Copy() {
            return new Coordinate(this.normal, this.binormal, this.front);
        }
    }

    void BuildBranch(int branchIndex, List<Vector3> vertices, List<int> triangles, List<Vector3> normals, List<Vector2> uv2, Vector3 offset, Coordinate _coord)
    {
        if(branchIndex >= branchNum) return;
        int segmentNum = (int)(branchSegment * (1f - (branchIndex / (float)branchNum))) + 1;
        segmentNum = (segmentNum / 2) * 2 + 1;
        if(branchIndex >= branchNum - 3) {
            GameObject leef = Instantiate(leaves, transform);
            leef.transform.position = offset + _coord.front / (float)branchNum * 0.5f;
            leef.transform.localScale = new Vector3(1f, 1f, 1f) * radius * 200f;
            leavesList.Add(leef.transform);
            leavesProgress.Add(branchIndex / (float)branchNum);
        }
        Coordinate coord = _coord.Copy();
        float delta = 0f;
        int indexOffset = vertices.Count;
        for(int i=0;i<segmentNum;i++) {
            for(int j=0;j<5;j++) {
                float rad = Mathf.PI * 2f / 5f * (float)j + delta;
                vertices.Add(
                    coord.normal * Mathf.Cos(rad) * radius +
                    coord.binormal * Mathf.Sin(rad) * radius +
                    offset
                );
                normals.Add(
                    coord.normal * Mathf.Cos(rad) +
                    coord.binormal * Mathf.Sin(rad)
                );
                uv2.Add(new Vector2(1f - (branchIndex + i / (float)(segmentNum - 1)) / (float)branchNum, 0f));
            }
            if(i == segmentNum - 1) break;
            offset += coord.front / (float)(branchNum * segmentNum);
            delta += Mathf.PI / 5f;

            coord.Rotate(branchDelta * (branchIndex / (float)branchNum));
        }
        for(int i=1;i<segmentNum;i++) {
            for(int j=0;j<5;j++) {
                triangles.Add(indexOffset + (i - 1) * 5 + j);
                triangles.Add(indexOffset + i * 5 + j);
                triangles.Add(indexOffset + (i - 1) * 5 + (j + 1) % 5);

                triangles.Add(indexOffset + i * 5 + (j + 1) % 5);
                triangles.Add(indexOffset + (i - 1) * 5 + (j + 1) % 5);
                triangles.Add(indexOffset + i * 5 + j);
            }
        }

        indexOffset += segmentNum * 5;

        BuildBranch(branchIndex + 1, vertices, triangles, normals, uv2,  offset, coord);
        BuildBranch(branchIndex + 1, vertices, triangles, normals, uv2, offset, coord);

    }

    Mesh FlatShading(Mesh mesh) {
        Vector3[] oldVertices = mesh.vertices;
        Vector3[] oldNormals = mesh.normals;
        Vector2[] oldUv2 = mesh.uv2;
		int[] newTriangles = mesh.triangles;
		Vector3[] newVertices = new Vector3[newTriangles.Length];
        Vector2[] newUv2 = new Vector2[newTriangles.Length];
        Vector2[] newUv3 = new Vector2[newTriangles.Length];

		for (int i = 0; i < newTriangles.Length; i++) 
		{
            int oldIndex = newTriangles[i];
			newVertices[i] = oldVertices[oldIndex];
			newTriangles[i] = i;
            newUv2[i] = new Vector2(oldUv2[oldIndex].x, oldNormals[oldIndex].x);
            newUv3[i] = new Vector2(oldNormals[oldIndex].y, oldNormals[oldIndex].z);
		}

        Mesh newMesh = new Mesh();
        newMesh.SetVertices(newVertices);
        newMesh.SetTriangles(newTriangles, 0);
        newMesh.RecalculateNormals();
        newMesh.SetUVs(1, newUv2);
        newMesh.SetUVs(2, newUv3);
        return newMesh;
    }

    Mesh BuildMesh() {

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv2 = new List<Vector2>();
        leavesList = new List<Transform>();
        leavesProgress = new List<float>();
        BuildBranch(0, vertices, triangles, normals, uv2, Vector3.zero, new Coordinate());
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices.ToArray());
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.SetNormals(normals.ToArray());
        mesh.SetUVs(1, uv2.ToArray());
        mesh = FlatShading(mesh);
        GetComponent<Renderer>().sharedMaterial.SetFloat("_Radius", radius);
        // mesh.RecalculateNormals();
        // mesh.SetIndices(mesh.GetIndices(0),MeshTopology.LineStrip,0);
        return mesh;

    }

    void Start() {
         foreach (Transform child in transform) {
            DestroyImmediate(child.gameObject);
        }
        GetComponent<MeshFilter>().sharedMesh = BuildMesh();
    }

    protected Mesh Build() {
        return BuildMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
