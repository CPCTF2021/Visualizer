using System;
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
        // ranking->rank->user (index = user.ranking - 1)
        static public List<List<User>> ranking = new List<List<User>>();
        bool changed = false;

        private void Start()
        {
            rankingArea = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (changed)
            {
                List<User> users = ranking.SelectMany(user => user) as List<User>;
                for (int i = 0; i < users.Count; i++)
                {
                    objs[i].SetPlayer(users[i].ranking, users[i].name, users[i].totalScore);
                }
                changed = false;
            }
        }

        public void AddUser(User user)
        {
            RankingEntry re = Instantiate(playerPrefab, rankingArea.content).GetComponent<RankingEntry>();
            objs.Add(re);

            ranking[user.ranking - 1].Add(user);

            changed = true;
        }

        public void Update(User before, User after)
        {
            if (before.id == after.id) throw new ArgumentException();

            ranking[before.ranking - 1].Remove(before);
            for (int i = before.ranking - 1; i > after.ranking; i--)
            {
                ranking[i + 1] = ranking[i];
            }
            ranking[after.ranking - 1].Add(after);

            changed = true;
        }
    }
}