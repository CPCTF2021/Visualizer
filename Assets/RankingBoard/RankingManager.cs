using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UserScripts;

namespace RankingScript
{
    public class RankingManager : MonoBehaviour
    {
        public RankingEntry[] rankingEntries;

        public class RankingUser : User
        {
            public RankingUser(User user)
            {
                id = user.id;
                name = user.name.ToUpper();
                icon = user.icon;
                scores = user.scores;
                totalScore = user.totalScore;
            }
            public int ranking = 0;
        }
        static public List<RankingUser> ranking = new List<RankingUser>(2000);
        bool changed = false;

        private void Start()
        {
            rankingEntries = GetComponentsInChildren<RankingEntry>();
        }

        private void Update()
        {
            if (changed)
            {
                for (int i = 0; i < 10 && i < ranking.Count; i++)
                {
                    rankingEntries[i].SetPlayer(ranking[i].ranking, ranking[i].name, ranking[i].totalScore);
                }
                changed = false;
            }
        }
        public void AddUser(User user)
        {
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
        public void Update(User data)
        {
            RankingUser ru = new RankingUser(data);
            ranking[ranking.FindIndex(user => user.id == data.id)] = ru;
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