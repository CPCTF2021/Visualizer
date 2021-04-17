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
        [SerializeField]
        GameObject pointObject;
        ControlTree controlTree;
        public string name, id;

        public Dictionary<Genre, float> scores = new Dictionary<Genre, float>();
        // これなんでint
        public int totalScore;
        public Dictionary<Genre, float> cumulativePercentage = new Dictionary<Genre, float>();

        public Texture icon;
        public void Initialize()
        {
            controlTree = GetComponent<ControlTree>();
            userIcon.SetUpVector(transform.position.normalized);
        }

        public float Gamma(float x)
        {
            return Mathf.Pow(x, 0.3f);
        }

        public void SetUser(string name, string id, Texture icon, Dictionary<Genre, float> scores)
        {
            this.name = name;
            this.id = id;
            this.icon = icon;
            AddScore(scores);

            controlTree.SetActive(true);
            // 10000fは最大ポイント
            controlTree.cumulativePercentage = cumulativePercentage;
            controlTree.AnimationTree(Gamma(totalScore / 10000f) * 0.7f + 0.3f, 1f);
            userIcon.gameObject.SetActive(true);
            userIcon.SetIcon(icon);
        }

        public void AddScore(Dictionary<Genre, float> scores)
        {
            foreach (var score in scores) this.scores.Add(score.Key, score.Value);
            totalScore = (int)Mathf.Ceil(this.scores.Sum(x => x.Value));
            float tmp = 0;
            foreach (Genre g in Enum.GetValues(typeof(Genre)))
            {
                tmp += scores[g];
                cumulativePercentage[g] = tmp / totalScore;
            }
        }

        public void AddScore(Genre genre, float score, float animationTime)
        {
            scores[genre] += score;
            totalScore += (int)Mathf.Ceil(score);
            float tmp = 0;
            foreach (Genre g in Enum.GetValues(typeof(Genre)))
            {
                tmp += scores[g];
                cumulativePercentage[g] = tmp / totalScore;
            }
            //10000fは最大ポイント
            controlTree.AnimationTree(Gamma(totalScore / 10000f) * 0.7f + 0.3f, animationTime);
            userIcon.AnimationIcon(animationTime);
            MakeScoreParticle(animationTime, genre);
        }

        void MakeScoreParticle(float animationTime, Genre genre)
        {
            UserPlusPoint particle = Instantiate(pointObject).GetComponent<UserPlusPoint>();
            particle.Initialize(transform.position, transform.position.normalized, animationTime, genre);
        }

        public Vector3 GetPosition()
        {
            return transform.position + transform.position.normalized * 0.7f;
        }
    }
}
