using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingList : MonoBehaviour
{
    public GameObject playerPrefab;
    public ScrollRect rankingArea;

    public int[] scores = new int[5];
    public string[] names = new string[5];

    public int id = 0;


    private void Start()
    {
        rankingArea = GetComponent<ScrollRect>();
    }

    public void AddPlayer()
    {
        GameObject p = Instantiate(playerPrefab, rankingArea.content);
        RankingEntry re = p.GetComponent<RankingEntry>();

        p.name = scores[id].ToString();
        re.SetPlayer(1, names[id], scores[id]);
        id++;
    }

}
