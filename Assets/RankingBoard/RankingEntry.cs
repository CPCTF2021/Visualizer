using UnityEngine;
using UnityEngine.UI;

namespace RankingScript
{
    public class RankingEntry : MonoBehaviour
    {
        public Text nameText;
        public Text scoreText;

        public void SetPlayer(string name, int score)
        {
            nameText.text = name;
            scoreText.text = score.ToString();
        }
    }
}
