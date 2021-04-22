using UnityEngine;
using UnityEngine.UI;

namespace RankingScript
{
    public class RankingEntry : MonoBehaviour
    {
        public Text rankText;
        public Text nameText;
        public Text scoreText;

        [ContextMenu("up font size")]
        public void UpFontSize()
        {
            int tmp = rankText.fontSize;
            tmp += 2;
            rankText.fontSize = tmp;
            nameText.fontSize = tmp;
            scoreText.fontSize = tmp;
        }

        public void SetPlayer(int rank, string name, int score)
        {
            rankText.text = rank.ToString();
            nameText.text = name;
            scoreText.text = score.ToString();
        }
    }
}
