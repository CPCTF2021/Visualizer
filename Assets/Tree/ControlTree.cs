using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static VisualizerSystem.ProblemSolvedEvent;
using static TreeScripts.TreeGenerator;

namespace TreeScripts
{
    public class ControlTree : MonoBehaviour
    {
        [SerializeField]
        Transform icon;

        [Range(0, 1)]
        float progress = 0f;

        Material material;
        public List<Transform> leaveList;
        public List<float> leaveProgress;
        public int branchNum;
        public float radius;
        public Dictionary<Genre, float> cumulativePercentage;
        bool isGrow = false;
        public void SetActive(bool flag)
        {
            isGrow = flag;
        }
        void GrowTree()
        {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            //木の設定
            progress = Mathf.Max(Mathf.Min(progress, 1f), 0f);
            props.SetFloat("_Radius", radius);
            props.SetFloat("_Progress", progress);
            GetComponent<MeshRenderer>().SetPropertyBlock(props);
            transform.localScale = new Vector3(progress, progress, progress);
            for (int i = 0; i < leaveList.Count; i++)
            {
                float scale = radius * 200f * Mathf.Max(Mathf.Min((progress - leaveProgress[i]) * branchNum * 0.3f, 1f), 0f);

                leaveList[i].localScale = new Vector3(scale, scale, scale);
            }

        }
        void LeaveColoring()
        {
            int index = 0;
            for (int i = 0; i < leaveList.Count; i++)
            {
                // 葉の設定
                MaterialPropertyBlock props2 = new MaterialPropertyBlock();
                while (index + 1 < 10 && cumulativePercentage[(Genre)(index + 1)] < i / (float)leaveList.Count)
                {
                    index++;
                }
                props2.SetColor("_Color", GENRE_TO_COLOR[index]);
                leaveList[i].gameObject.GetComponent<MeshRenderer>().SetPropertyBlock(props2);
            }
        }

        public void ResetTree()
        {
            material = GetComponent<Renderer>().material;
            // 木をprogress0にリセット
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            props.SetFloat("_Radius", radius);
            props.SetFloat("_Progress", 0f);
            GetComponent<MeshRenderer>().SetPropertyBlock(props);
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
    }

}
