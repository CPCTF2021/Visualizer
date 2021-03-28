using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            public int genre;
            public int point;

        }
        Queue<UserQueueData> userQueue = new Queue<UserQueueData>();

        IEnumerator LoadUsers()
        {
            Texture tex = Resources.Load("icon") as Texture;
            for (int i = 0; i < this.users.Count; i++)
            {
                AddUser("UserName", i.ToString(), tex);
            }

            for (int i = 0; i < 500; i++)
            {
                AddPoint(UnityEngine.Random.Range(0, this.users.Count).ToString(), UnityEngine.Random.Range(0, 10), 2000);
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 5f));
            }
        }

        // 木に付いてるUserを取得
        public void SetTree()
        {
            this.users = new List<User>();
            this.usedTree = new bool[treeParent.childCount];
            for (int i = 0; i < treeParent.childCount; i++)
            {
                this.users.Add(treeParent.GetChild(i).GetComponent<User>());
                this.users[i].Initialize();
                this.usedTree[i] = false;
            }
            this.usersDictionary = new Dictionary<string, User>();

            // テストコード
            //StartCoroutine(LoadUsers());
        }

        // Userの新規追加
        public void AddUser(string name, string id, Texture icon, int[] pointsNum)
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

            Points points = new Points();
            for (int i = 0; i < 10; i++)
            {
                points.Add(i, pointsNum[i]);
            }

            users[index].SetUser(name, id, icon, points);
            usersDictionary.Add(id, users[index]);
        }

        // Userの新規追加(得点0)
        public void AddUser(string name, string id, Texture icon)
        {
            this.AddUser(name, id, icon, new int[10]);
        }

        // ポイントを加える
        public void AddPoint(string id, int genre, int point)
        {
            User user;
            if (!usersDictionary.TryGetValue(id, out user)) throw new MissingFieldException();
            UserQueueData userQueueData;
            userQueueData.user = user;
            userQueueData.genre = genre;
            userQueueData.point = point;
            userQueue.Enqueue(userQueueData);
            if (userQueue.Count == 1) StartCoroutine(DoAnimation());
        }

        // ポイントを加えるアニメーションの実体
        IEnumerator DoAnimation()
        {
            while (userQueue.Count > 0)
            {
                UserQueueData userQueueData = userQueue.Dequeue();
                userQueueData.user.AddPoint(userQueueData.genre, userQueueData.point);
                cameraAnimator.SetTarget(userQueueData.user.GetPosition());
                yield return new WaitForSeconds(5f / (float)userQueue.Count);
            }
        }
    }
}
