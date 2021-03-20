using UnityEngine;
using UserScripts;
using TreeScripts;

namespace VisualizerSystem
{
    public class SystemInitializer : MonoBehaviour
    {
        // leaveの色変換は一時的なもの
        [SerializeField]
        Color leaveBaseColor;
        [SerializeField]
        float baseColorRate = 0.5f;
        void Start()
        {
            // 葉の色
            for(int i=0;i<Points.GENRE_TO_COLOR.Length;i++)
            {
                Points.GENRE_TO_COLOR[i] = Points.GENRE_TO_COLOR[i] * 0.95f + new Color(1f, 1f, 1f) * 0.05f;
                Points.GENRE_TO_COLOR[i] = Points.GENRE_TO_COLOR[i] * baseColorRate + leaveBaseColor * (1f - baseColorRate);
                // Points.GENRE_TO_COLOR[i] = Mathf.Pow(new Color(1f, 1f, 1f) - Points.GENRE_TO_COLOR[i], 2.0f);
            }

            GetComponent<TreeGenerator>().MakeTree();
            GetComponent<UserManager>().SetTree();
        }
    }
}
