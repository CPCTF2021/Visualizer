using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingEntry : MonoBehaviour
{
    public Text rankText;
    public Text nameText;
    public Text scoreText;

    public void setPlayer(int rank, string name, int score)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
