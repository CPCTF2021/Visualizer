using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserScripts;

namespace RankingScript
{
    public class RankingManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public ScrollRect rankingArea;

        public RankingEntry[] rankingEntries;

        public List<RankingEntry> objs;
        static public List<User> ranking = new List<User>(2000);
        bool changed = false;

        private void Start()
        {
            // rankingArea = GetComponent<ScrollRect>();

            rankingEntries = GetComponentsInChildren<RankingEntry>();
        }

        private void Update()
        {
            if (changed)
            {
                for (int i = 0; i < 10; i++)
                {
                    // objs[i].SetPlayer(ranking[i].ranking, ranking[i].name, ranking[i].totalScore);
                    rankingEntries[i].SetPlayer(ranking[i].ranking, ranking[i].name, ranking[i].totalScore);
                }
                changed = false;
            }
        }

        public void AddUser(User user)
        {
            // GameObject a = Instantiate(playerPrefab, rankingArea.content);
            // RankingEntry re = a.GetComponent<RankingEntry>();
            if (user.ranking <= 10)
            {
                rankingEntries[user.ranking - 1].SetPlayer(user.ranking, user.name, user.totalScore);
            }
            // objs.Add(re);

            ranking.Add(user);

            changed = true;
        }

        public void Update(User data)
        {
            ranking[ranking.FindIndex(user => user.id == data.id)] = data;

            ranking.Sort((a, b) => b.totalScore - a.totalScore);

            var rank = 0;
            var count = 0;
            var score = 10000;
            foreach (var user in ranking)
            {
                if (user.totalScore == score)
                {
                    user.ranking = rank;
                    count++;
                }
                else
                {
                    score = user.totalScore;
                    rank += count; count = 0;
                    rank++;
                    user.ranking = rank;
                }
            }
            changed = true;
        }
    }
}