﻿using UnityEngine;
using UserScripts;
using TreeScripts;
using RankingScript;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using static VisualizerSystem.ProblemSolvedEvent;
using System.Linq;
using System.Threading;

namespace VisualizerSystem
{
    public class SystemInitializer : MonoBehaviour
    {
        UserManager userManager;
        EventManager eventManager = new EventManager();
        RankingManager rankingManager;
        Timer timer;
        static string BASE_URL = "https://cpctf.space";
        bool flag = false;
        async void Start()
        {
            // GetComponent<TreeGenerator>().MakeTree();
            userManager = GetComponent<UserManager>();
            userManager.Initialize();
            rankingManager = GameObject.Find("RankingPanel").GetComponent<RankingManager>();
            timer = GameObject.Find("Timer").GetComponent<Timer>();
            await Sync();
            
#if !UNITY_EDITOR

            eventManager.Init();

            TimeAdjusterEvent timeAdjusterEvent = new TimeAdjusterEvent(userManager, timer);
            eventManager.Register(timeAdjusterEvent.Handler);

            UserCreatedEvent userCreatedEvent = new UserCreatedEvent(userManager, rankingManager);
            eventManager.Register(userCreatedEvent.Handler);

            ProblemSolvedEvent problemSolvedEvent = new ProblemSolvedEvent(userManager, rankingManager);
            eventManager.Register(problemSolvedEvent.Handler);
#endif
            flag = true;
        }
        void Update()
        {
#if !UNITY_EDITOR
            if(flag) eventManager.Handle();
#endif
        }
        void OnDestroy()
        {
#if !UNITY_EDITOR
            eventManager.Shutdown();
#endif
        }
        [Serializable]
        public class UserResponse
        {
            public string id;
            public string name;
            public string iconUrl;
            public List<Score> scores;
        }
        [Serializable]
        public class Score
        {
            public Genre genre;
            public float score;
        }

        // async void Sync()
        // {
        //     List<UserResponse> res = await FetchUsers();
        //     List<User> users = new List<User>(res.Count);
        //     var tasks = new List<Task<Texture2D>>();
        //     var semaphore = new SemaphoreSlim(30, 30);
        //     foreach (UserResponse user in res)
        //     {
        //         await semaphore.WaitAsync();
        //         tasks.Add(FetchIcon(user.iconUrl));
        //         semaphore.Release(1);
        //     }
        //     var icons = await Task.WhenAll(tasks);
        //     for (int i = 0; i < users.Count; i++)
        //     {
        //         var scores = new Dictionary<Genre, float>();
        //         foreach (Score score in res[i].scores)
        //         {
        //             scores.Add(score.genre, score.score);
        //         }
        //         var tmp = new User();
        //         Texture2D texture = new Texture2D(5, 5);
        //         tmp.name = res[i].name;
        //         tmp.id = res[i].id;
        //         tmp.icon = icons[i];
        //         tmp.scores = scores;
        //         users.Add(tmp);
        //     }
        //     try { userManager.AddUsers(users); }
        //     catch (MissingFieldException err) { Debug.LogError(err); }
        //     try { rankingManager.AddUsers(userManager.users.Where(user => user.id != "").ToList()); }
        //     catch (ArgumentException err) { Debug.LogError(err); }
        // }
        async Task Sync()
        {
            List<UserResponse> res = await FetchUsers();
            List<User> users = new List<User>(res.Count);
            foreach (UserResponse user in res)
            {
                var scores = new Dictionary<Genre, float>();
                foreach (Score score in user.scores)
                {
                    scores.Add(score.genre, score.score);
                }
                var tmp = new User();
                Texture2D texture = new Texture2D(5, 5);
                tmp.name = user.name;
                tmp.id = user.id;
                tmp.scores = scores;
                users.Add(tmp);
                FetchIcon(user.id, user.iconUrl);
            }
            try { userManager.AddUsers(users); }
            catch (MissingFieldException err) { Debug.LogError(err); }
            try { rankingManager.AddUsers(userManager.users.Where(user => user.id != "").ToList()); }
            catch (ArgumentException err) { Debug.LogError(err); }
        }
        public async Task<List<UserResponse>> FetchUsers()
        {
            using (UnityWebRequest req = UnityWebRequest.Get(BASE_URL + "/api/visualizer"))
            {
                await req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(BASE_URL + "/api/visualizer" + ": Error: " + req.error);
                    return new List<UserResponse>();
                }
                else
                {
                    Debug.Log("SUCCESS: " + BASE_URL + "/api/visualizer");
                    return JsonHelper.FromJson<UserResponse>(req.downloadHandler.text);
                }
            }
        }
        public async void FetchIcon(string id, string iconUrl)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(iconUrl))
            {
                await req.SendWebRequest();

                var tex = new Texture2D(2, 2);
                if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(iconUrl + ": Error: " + req.error);
                }
                else
                {
                    Debug.Log("SUCCESS: " + iconUrl);
                    if (!tex.LoadImage(req.downloadHandler.data))
                    {
                        Debug.LogError(iconUrl + ": IconLoadError");
                    }
                }
                userManager.AddUserIcon(id, tex);
            }
        }
    }
}