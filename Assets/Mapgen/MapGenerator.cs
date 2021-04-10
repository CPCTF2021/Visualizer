using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    float heightStrength = 0.1f;
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }
    public static float noise3D(Vector3 p)
    {
        float x = p.x;
        float y = p.y;
        float z = p.z;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
    
        return xy * xz * yz * yx * zx * zy;
    }
    public static float FBM(Vector3 p)
    {
        p += new Vector3(23.3f, 13.7f, 34.5f) * 100.0f;
        Matrix4x4 mat;
        mat = new Matrix4x4();
        mat.SetColumn(0, new Vector4(1f, 0.8f, 0.6f, 0f));
        mat.SetColumn(1, new Vector4(-0.8f, 1f, 0.48f, 0f));
        mat.SetColumn(2, new Vector4(-0.6f, -0.48f, 1f, 0f));
        mat.SetColumn(3, new Vector4(23.3f, 13.7f, 34.5f, 1f));

        float a = 0.5f;
        float result = 0f;
        for(int i=0;i<5;i++)
        {
            result += noise3D(p * Mathf.Pow(1.2f, i)) * a;
            a *= 0.5f;
            p = (Vector3)(mat * new Vector4(p.x, p.y, p.z, 1f));
        }

        result += Mathf.Max(0.0f, 1f - result * 3.0f) * noise3D(p * Mathf.Pow(1.2f, 6)) * 0.3f;
        return result;
    }

    [ContextMenu("BuildMesh")]
    void BuildMesh()
    {

        Mesh mesh = GetComponent<MeshFilter>().mesh;

        List<Vector3> vertices = new List<Vector3>();
        mesh.GetVertices(vertices);

        for(int i=0;i<vertices.Count;i++)
        {
            Vector3 pos = vertices[i];
            float noise = 1f - FBM(pos * 97.7f);
            float radius = pos.magnitude;
            Vector3 normal = pos / radius;

            // vertices[i] = new Vector3(pos.x, pos.y, noise / 100f * heightStrength);
            vertices[i] = normal * (heightStrength * (noise - 0.5f) + 1f) * radius;
        }
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
