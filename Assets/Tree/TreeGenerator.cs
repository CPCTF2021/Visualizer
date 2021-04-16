using UnityEngine;

namespace TreeScripts
{
    public class TreeGenerator : MonoBehaviour
    {
        [SerializeField]
        public static Color[] GENRE_TO_COLOR = new Color[10]{
            new Color(0 / 256f, 171 / 256f, 214 / 256f),
            new Color(0 / 256f, 216 / 256f, 133 / 256f),
            new Color(137 / 256f, 91 / 256f, 0 / 256f),
            new Color(173 / 256f, 166 / 256f, 145 / 256f),
            new Color(177 / 256f, 249 / 256f, 114 / 256f),
            new Color(150 / 256f, 200 / 256f, 255 / 256f),
            new Color(219 / 256f, 43 / 256f, 0 / 256f),
            new Color(198 / 256f, 198 / 256f, 198 / 256f),
            new Color(125 / 256f, 0 / 256f, 188 / 256f),
            new Color(0 / 256f, 38 / 256f, 255 / 256f),
        };

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

        public void MakeTree()
        {
            TreeMesh treeMesh = new TreeMesh(segmentNum, branchNum, branchLength, stemRadius);
            treeMesh.BuildMesh();
            if (treeParent.childCount == 0)
            {
                // 自動配置
                float phi = 0f;
                for (int i = 0; i < num; i++)
                {
                    GameObject t = Instantiate(tree, treeParent);
                    treeMesh.SetMesh(t.GetComponent<ControlTree>(), leave);
                    float h = 2f * i / (num - 1f) - 1f;
                    float theta = Mathf.Acos(h);
                    if (Mathf.Abs(h) != 1f) phi = phi + 3.6f / Mathf.Sqrt(num * (1f - h * h));
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
            }
            else
            {
                // すでに配置されてるのをActiveに
                num = treeParent.childCount;
                for (int i = 0; i < num; i++)
                {
                    GameObject t = treeParent.GetChild(i).gameObject;
                    treeMesh.SetMesh(t.GetComponent<ControlTree>(), leave);
                }
            }
        }
    }
}
