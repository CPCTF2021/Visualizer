using UnityEngine;
using UnityEngine.UI;

namespace RankingBoard {
    public class RankingEntry : MonoBehaviour
    {
        public Text rankText;
        public Text nameText;
        public Text scoreText;

        public void SetPlayer(int rank, string name, int score)
        {
            rankText.text = rank.ToString();
            nameText.text = name;
            scoreText.text = score.ToString();
        }
    }
}
