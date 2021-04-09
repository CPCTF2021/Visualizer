using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static VisualizerSystem.ProblemSolvedEvent;

namespace TreeScripts
{
    public class ControlTree : MonoBehaviour
    {
        [SerializeField]
        Transform icon;

        [Range(0, 1)]
        float progress = 0f;
        float prevProgress = 0f;

        Material material;
        public List<Transform> leaveList;
        public List<float> leaveProgress;
        public int branchNum;
        public float radius;
        public Dictionary<Genre, float> cumulativePercentage;
        bool isGrow = false;

        Sequence sequence;

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
            prevProgress = progress;

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

        public void AnimationTree(float progress)
        {
            if (!isGrow) return;
            if (sequence != null) sequence.Kill();
            LeaveColoring();
            sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            Vector3 scale = new Vector3(1f, 1f, 1f) * 0.1f;
            sequence.Append(icon.DOScale(scale * 3f, 0.5f).SetEase(Ease.OutExpo));
            sequence.Append(icon.DOScale(scale, 0.5f).SetEase(Ease.OutExpo));
            // 木のアニメーション
            DOTween.To(() => this.progress, (val) =>
            {
                this.progress = val;
                GrowTree();
            }, progress, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.5f);
        }
    }

}
