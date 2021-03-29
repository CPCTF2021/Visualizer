using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingList : MonoBehaviour
{
    public GameObject playerPrefab;
    public ScrollRect rankingArea;

    public class user
    {
        public int score;
        public int rank;
        public string name;
    }

    public List<RankingEntry> objs;
    public List<user> users;




    public int[] scores = new int[5];
    public string[] names = new string[5];

    public int id = 0;

    public static int compareScore(user user1, user user2)
    {
        if (user1 == null)
        {
            if (user2 == null)
            {
                return 0;
            }
            return 1;
        }
        else
        {
            if (user2 == null)
            {
                return -1;
            }

            return user2.score.CompareTo(user1.score);
        }
    }


    private void Start()
    {
        rankingArea = GetComponent<ScrollRect>();
        users = new List<user>();
    }

    private void Update()
    {

    }

    public void addPlayer()
    {
        GameObject p = Instantiate(playerPrefab, rankingArea.content);
        RankingEntry re = p.GetComponent<RankingEntry>();
        objs.Add(re);

        p.name = scores[id].ToString();
        re.setPlayer(id + 1, names[id], scores[id]);
        id++;
        user t = new user();
        t.name = re.myName;
        t.rank = re.myRank;
        t.score = re.myScore;
        users.Add(t);
    }


    public void sortRankingList()
    {
        users.Sort(compareScore);
        for (int i = 0; i < users.Count; i++)
        {
            objs[i].setPlayer(i + 1, users[i].name, users[i].score);
        }
    }
}
