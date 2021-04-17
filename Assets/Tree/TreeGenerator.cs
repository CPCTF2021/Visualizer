using UnityEngine;

namespace TreeScripts
{
    public class TreeGenerator : MonoBehaviour
    {
        [SerializeField]
        public static Color[] GENRE_TO_COLOR = new Color[10]{
            new Color(1f, 0.4509804f, 0.5137255f),
            new Color(1, 0.6901961f, 0.50980395f),
            new Color(1, 0.89411765f, 0.47058824f),
            new Color(1, 0.9843137f, 0.5294118f),
            new Color(0.8666667f, 1, 0.6392157f),
            new Color(0.60784316f, 1, 0.49019608f),
            new Color(0.44705883f, 0.7294118f, 0.5254902f),
            new Color(0.45490196f, 0.6666667f, 0.9098039f),
            new Color(0.8784314f, 0.7019608f, 1f),
            new Color(0.8117647f, 0.37254903f, 0.7647059f),
        };
        [SerializeField]
        GameObject leave;
        [SerializeField]
        Transform treeParent;
        [SerializeField]
        int num, segmentNum, branchNum;
        [SerializeField]
        float radius, branchLength, stemRadius;

        TreeMesh treeMesh;

        public void MakeTree()
        {
            treeMesh = new TreeMesh(segmentNum, branchNum, branchLength, stemRadius);
            treeMesh.BuildMesh();
        }

        public void SetMesh(ControlTree controlTree)
        {
            treeMesh.SetMesh(controlTree, leave);
        }
    }
}
