using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static VisualizerSystem.ProblemSolvedEvent;
using static TreeScripts.TreeGenerator;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TreeScripts
{
    public class ControlTree : MonoBehaviour
    {
        [SerializeField]
        Transform icon;

        [SerializeField, Range(0, 1)]
        float progress = 0f;

        Material material;
        public List<Transform> leaveList;
        public List<float> leaveProgress;
        public int branchNum;
        public float radius;
        public Dictionary<Genre, float> cumulativePercentage;
        bool isGrow = false;

        [SerializeField]
        Vector3 originalScale;

        MaterialPropertyBlock treeProperty;
        List<MaterialPropertyBlock> leaveProperties;

        void Start() {
            originalScale = transform.localScale;
            this.progress = 0f;
            treeProperty = new MaterialPropertyBlock();
            leaveProperties = new List<MaterialPropertyBlock>();
            leaveList.ForEach((obj) => {
                leaveProperties.Add(new MaterialPropertyBlock());
            });
            GrowTree();
        }

        public void SetActive(bool flag)
        {
            isGrow = flag;
        }
        void GrowTree()
        {
            treeProperty.Clear();
            //木の設定
            progress = Mathf.Max(Mathf.Min(progress, 1f), 0f);
            treeProperty.SetFloat("_Radius", radius);
            treeProperty.SetFloat("_Progress", progress);
            GetComponent<MeshRenderer>().SetPropertyBlock(treeProperty);
            transform.localScale = Vector3.Scale(new Vector3(progress, progress, progress), originalScale);
            for (int i = 0; i < leaveList.Count; i++)
            {
                float scale = radius * 200f * Mathf.Max(Mathf.Min((progress - leaveProgress[i]) * branchNum * 0.3f, 1f), 0f);

                leaveList[i].localScale = Vector3.Scale(new Vector3(scale, scale, scale), originalScale);
            }

        }
        void LeaveColoring()
        {
            int index = 0;
            for (int i = 0; i < leaveList.Count; i++)
            {
                // 葉の設定
                leaveProperties[i].Clear();
                while (index + 1 < 10 && cumulativePercentage[(Genre)(index + 1)] < i / (float)leaveList.Count)
                {
                    index++;
                }
                leaveProperties[i].SetColor("_Color", GENRE_TO_COLOR[index]);
                leaveList[i].gameObject.GetComponent<MeshRenderer>().SetPropertyBlock(leaveProperties[i]);
            }
        }

        public void ResetTree()
        {
            material = GetComponent<Renderer>().material;
            // 木をprogress0にリセット
            treeProperty.Clear();
            treeProperty.SetFloat("_Radius", radius);
            treeProperty.SetFloat("_Progress", 0f);
            GetComponent<MeshRenderer>().SetPropertyBlock(treeProperty);
            transform.localScale = Vector3.zero;
            for (int i = 0; i < leaveList.Count; i++)
            {
                float scale = radius * 200f * Mathf.Max(Mathf.Min((0f - leaveProgress[i]) * (float)branchNum * 0.3f, 1f), 0f);

                leaveList[i].localScale = new Vector3(scale, scale, scale);
            }
        }

        public void AnimationTree(float progress, float animationTime)
        {
            if (!isGrow) return;
            LeaveColoring();
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.3f * animationTime);
            // 木のアニメーション
            sequence.Append(DOTween.To(() => this.progress, (val) =>
            {
                this.progress = val;
                GrowTree();
            }, progress, 0.7f * animationTime).SetEase(Ease.OutQuart));
        }
#if UNITY_EDITOR
        [ContextMenu("SaveMesh")]
        void SaveMesh()
        {
            AssetDatabase.CreateAsset(GetComponent<MeshFilter>().mesh, $"Assets/TreeGenerator/Resources/{name}.asset");
            AssetDatabase.SaveAssets();
        }
        public void SetProgress(float p) {
            Debug.Log(p);
            this.progress = p;
            originalScale = new Vector3(1f, 1f, 1f);
            GrowTree();
        }
#endif
    }

}
