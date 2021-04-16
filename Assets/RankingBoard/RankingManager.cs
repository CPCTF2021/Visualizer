using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UserScripts;

namespace RankingScript
{
    public class RankingManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public ScrollRect rankingArea;

        public List<RankingEntry> objs;
        public class RankingUser : User
        {
            public RankingUser(User user)
            {
                id = user.id;
                name = user.name;
                icon = user.icon;
                scores = user.scores;
            }
            public int ranking = 0;
        }
        static public List<RankingUser> ranking = new List<RankingUser>(2000);
        bool changed = false;

        private void Start()
        {
            rankingArea = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (changed)
            {
                for (int i = 0; i < ranking.Count; i++)
                {
                    objs[i].SetPlayer(ranking[i].ranking, ranking[i].name, ranking[i].totalScore);
                }
                changed = false;
            }
        }
        public void AddUser(User user)
        {
            GameObject a = Instantiate(playerPrefab, rankingArea.content);
            RankingEntry re = a.GetComponent<RankingEntry>();
            objs.Add(re);

            ranking.Add(new RankingUser(user));
            Sort();
            changed = true;
        }
        public void AddUsers(List<User> users)
        {
            ranking = users.Select(user => new RankingUser(user)).ToList();
            Sort();
            changed = true;
        }
        public void Update(RankingUser data)
        {
            ranking[ranking.FindIndex(user => user.id == data.id)] = data;
            Sort();
            changed = true;
        }
        private void Sort()
        {
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
        }
    }
}