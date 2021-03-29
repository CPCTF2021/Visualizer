using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingEntry : MonoBehaviour
{
    public Text rankText;
    public Text nameText;
    public Text scoreText;

    public int myRank;
    public string myName;
    public int myScore;

    public void setPlayer(int rank, string name, int score)
    {
        rankText.text = rank.ToString();
        myRank = rank;

        nameText.text = name;
        myName = name;

        scoreText.text = score.ToString();
        myScore = score;
    }
}
