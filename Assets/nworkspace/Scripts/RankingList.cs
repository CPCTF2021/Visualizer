using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingList : MonoBehaviour
{
    public GameObject playerPrefab;
    public ScrollRect rankingArea;

    public List<Player> players = new List<Player>();



    private void Start()
    {
        rankingArea = GetComponent<ScrollRect>();
    }

    private void Update()
    {

    }

    public void addPlayer()
    {
        GameObject p = Instantiate(playerPrefab, rankingArea.content);
        RankingEntry re = p.GetComponent<RankingEntry>();
        re.setPlayer(1, "ntaso", 500000);
    }

}
