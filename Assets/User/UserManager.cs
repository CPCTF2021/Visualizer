using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VisualizerSystem.ProblemSolvedEvent;
using CameraScripts;
using TreeScripts;

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

        struct UserQueueData
        {
            public User user;
            public Genre genre;
            public float score;
        }
        Queue<UserQueueData> userQueue = new Queue<UserQueueData>();

        bool isAnimation = false;
        NotificationManager notificationManager;
        TreeGenerator treeGenerator;

        // 木に付いてるUserを取得
        public void Initialize()
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
            notificationManager = GameObject.Find("Notification").GetComponent<NotificationManager>();
        }

        // 既存Userの反映
        public void AddUser(string name, string id, Texture icon, Dictionary<Genre, float> scores)
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

            users[index].GetComponent<ControlTree>().ResetTree();
            users[index].SetUser(name, id, icon, scores);
            usersDictionary.Add(id, users[index]);
        }

        public void AddUsers(List<User> users)
        {
            foreach (User user in users)
            {
                AddUser(user.name, user.id, user.icon, user.scores);
            }
        }

        // 新規Userの追加
        public void AddUser(string name, string id, Texture icon)
        {
            var zero = new Dictionary<Genre, float>();
            foreach (Genre g in Enum.GetValues(typeof(Genre))) zero[g] = 0;
            AddUser(name, id, icon, zero);
        }

        // ポイントを加える
        public void AddScore(string id, Genre genre, float score)
        {
            User user;
            if (!usersDictionary.TryGetValue(id, out user)) {
                throw new MissingFieldException();
            }
            usersDictionary[id].scores[genre] += score;
            usersDictionary[id].totalScore += (int)score;
            UserQueueData userQueueData;
            userQueueData.user = user;
            userQueueData.genre = genre;
            userQueueData.score = score;
            userQueue.Enqueue(userQueueData);
            if (!isAnimation && userQueue.Count == 1) StartCoroutine(DoAnimation());
        }

        // ポイントを加えるアニメーションの実体
        IEnumerator DoAnimation()
        {
            isAnimation = true;
            cameraAnimator.MoveToTarget(2f);
            UserQueueData userQueueData = userQueue.Dequeue();
            cameraAnimator.ChangeTarget(userQueueData.user.GetPosition());
            yield return new WaitForSeconds(2f);
            float t;
            while (true)
            {
                t = Mathf.Max(Mathf.Exp(-userQueue.Count * 0.5f) * 1.3f, 0.2f);
                userQueueData.user.AddScore(userQueueData.genre, userQueueData.score, t);
                notificationManager.Add(userQueueData.user.name, userQueueData.genre, userQueueData.score);
                yield return new WaitForSeconds(t);
                if (userQueue.Count == 0) break;
                userQueueData = userQueue.Dequeue();
                cameraAnimator.ChangeTarget(userQueueData.user.GetPosition());
            }
            cameraAnimator.LeaveFromTarget(1f);
            yield return new WaitForSeconds(1f);
            if(userQueue.Count >= 1) yield return DoAnimation();
            isAnimation = false;
        }
    }
}
