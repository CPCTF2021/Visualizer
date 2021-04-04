using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VisualizerSystem.ProblemSolvedEvent;

namespace UserScripts
{
    public class UserManager : MonoBehaviour
    {
        [SerializeField]
        Transform treeParent;
        public List<User> users;
        public bool[] usedTree;
        public Dictionary<string, User> usersDictionary;

        [SerializeField]
        CameraAnimator cameraAnimator;

        int lastPlace = 1;

        struct UserQueueData
        {
            public User user;
            public Genre genre;
            public float score;
        }
        Queue<UserQueueData> userQueue = new Queue<UserQueueData>();

        // 木に付いてるUserを取得
        public void SetTree()
        {
            users = new List<User>();
            usedTree = new bool[treeParent.childCount];
            for (int i = 0; i < treeParent.childCount; i++)
            {
                users.Add(treeParent.GetChild(i).GetComponent<User>());
                users[i].Initialize();
                usedTree[i] = false;
            }
            usersDictionary = new Dictionary<string, User>();
        }

        // 既存Userの反映
        public void AddUser(string name, string id, Texture icon, Dictionary<Genre, float> scores, int ranking)
        {
            int count = users.Count;
            int index = UnityEngine.Random.Range(0, count);

            bool fullyUsed = true;

            for (int i = 0; i < count; i++)
            {
                if (!usedTree[(index + i) % count])
                {
                    index = (index + i) % count;
                    usedTree[index] = true;
                    fullyUsed = false;
                    break;
                }
            }

            if (fullyUsed) throw new IndexOutOfRangeException();

            // 起動時に、木にユーザーを割当するため
            users[index].SetUser(name, id, icon, scores, ranking);
            usersDictionary.Add(id, users[index]);

            if (ranking > lastPlace) lastPlace = ranking;
        }

        // 新規Userの追加
        public void AddUser(string name, string id, Texture icon)
        {
            var zero = new Dictionary<Genre, float>();
            foreach (Genre g in Enum.GetValues(typeof(Genre))) zero[g] = 0;
            AddUser(name, id, icon, zero, lastPlace);
        }

        // ポイントを加える
        public void AddScore(string id, Genre genre, float score)
        {
            User user;
            if (!usersDictionary.TryGetValue(id, out user)) throw new MissingFieldException();
            UserQueueData userQueueData;
            userQueueData.user = user;
            userQueueData.genre = genre;
            userQueueData.score = score;
            userQueue.Enqueue(userQueueData);
            if (userQueue.Count == 1) StartCoroutine(DoAnimation());
        }

        // ポイントを加えるアニメーションの実体
        IEnumerator DoAnimation()
        {
            while (userQueue.Count > 0)
            {
                UserQueueData userQueueData = userQueue.Dequeue();
                userQueueData.user.AddScore(userQueueData.genre, userQueueData.score);
                cameraAnimator.SetTarget(userQueueData.user.GetPosition());
                yield return new WaitForSeconds(5f / userQueue.Count);
            }
        }
    }
}
