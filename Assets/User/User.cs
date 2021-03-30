using UnityEngine;
using TreeScripts;
using System.Collections.Generic;
using static VisualizerSystem.ProblemSolvedEvent;
using System.Linq;
using System;

namespace UserScripts
{
    public class User : MonoBehaviour
    {
        [SerializeField]
        UserIcon userIcon;
        ControlTree controlTree;
        public string name, id;

        public int ranking;

        public Dictionary<Genre, float> scores;
        public int totalScore;
        public Dictionary<Genre, float> cumulativeParcentage;

        public Texture icon;
        public void Initialize()
        {
            controlTree = GetComponent<ControlTree>();
        }

        public void SetUser(string name, string id, Texture icon, Dictionary<Genre, float> scores, int ranking)
        {
            this.name = name;
            this.id = id;
            this.icon = icon;
            AddScore(scores);
            this.ranking = ranking;

            controlTree.SetActive(true);
            //TODO: 10000fは最大ポイント 
            controlTree.AnimationTree(totalScore / 10000f * 0.7f + 0.3f);
            userIcon.gameObject.SetActive(true);
            userIcon.SetIcon(icon);
        }

        public void AddScore(Dictionary<Genre, float> scores)
        {
            foreach(var score in scores)
            {
                this.scores[score.Key] += score.Value;
            }
            totalScore = (int)Mathf.Ceil(scores.Skip(1).Sum(x => x.Value));
            float tmp = 0;
            foreach (Genre g in Enum.GetValues(typeof(Genre)))
            {
                tmp += scores[g];
                cumulativeParcentage[g] = tmp / totalScore;
            }
        }

        public void AddScore(Genre genre, float score)
        {
            scores[genre] += score;
            //TODO: 10000fは最大ポイント
            controlTree.AnimationTree(totalScore / 10000f * 0.7f + 0.3f);
        }

        public Vector3 GetPosition()
        {
            return transform.position + transform.rotation * Vector3.up * 0.7f;
        }
    }
}
