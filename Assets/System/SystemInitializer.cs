using UnityEngine;
using UserScripts;
using TreeScripts;
using RankingScript;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using static VisualizerSystem.ProblemSolvedEvent;

namespace VisualizerSystem
{
    public class SystemInitializer : MonoBehaviour
    {
        static UserManager userManager;
        EventManager eventManager = new EventManager();
        static RankingManager rankingManager;
        static string BASE_URL = "https://cpctf.space";
        void Start()
        {
            GetComponent<TreeGenerator>().MakeTree();
            userManager = GetComponent<UserManager>();
            userManager.Initialize();
            rankingManager = GameObject.Find("RankingPanel").GetComponent<RankingManager>();
#if !UNITY_EDITOR
            Sync();

            eventManager.Init();

            TimeAdjusterEvent timeAdjusterEvent = new TimeAdjusterEvent(userManager);
            eventManager.Register(timeAdjusterEvent.Handler);

            UserCreatedEvent userCreatedEvent = new UserCreatedEvent(userManager, rankingManager);
            eventManager.Register(userCreatedEvent.Handler);

            ProblemSolvedEvent problemSolvedEvent = new ProblemSolvedEvent(userManager, rankingManager);
            eventManager.Register(problemSolvedEvent.Handler);
#endif
        }
        void Update()
        {
#if !UNITY_EDITOR
            eventManager.Handle();
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
            public string iconURL;
            public List<Score> scores;
        }
        [Serializable]
        public class Score
        {
            public Genre genre;
            public float score;
        }
        async void Sync()
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
                tmp.SetUser(user.name, user.id, await FetchIcon(user.iconURL), scores);
                users.Add(tmp);
            }
            try { userManager.AddUsers(users); }
            catch (MissingFieldException err) { Debug.LogError(err); }
            try { rankingManager.AddUsers(users); }
            catch (ArgumentException err) { Debug.LogError(err); }
        }
        public async Task<List<UserResponse>> FetchUsers()
        {
            using (UnityWebRequest req = UnityWebRequest.Get(BASE_URL + "/users"))
            {
                await req.SendWebRequest();

                if (req.isHttpError || req.isNetworkError)
                {
                    Debug.LogError(BASE_URL + "/users" + ": Error: " + req.error);
                    return new List<UserResponse>();
                }
                else
                {
                    Debug.Log("SUCCESS: " + BASE_URL + "/users");
                    return JsonHelper.FromJson<UserResponse>(req.downloadHandler.text);
                }
            }
        }
        public async Task<Texture2D> FetchIcon(string iconUrl)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(iconUrl))
            {
                await req.SendWebRequest();

                var tex = new Texture2D(2, 2);
                if (req.isHttpError || req.isNetworkError)
                {
                    Debug.LogError(BASE_URL + "/users" + ": Error: " + req.error);
                }
                else
                {
                    Debug.Log("SUCCESS: " + BASE_URL + "/users");
                    if (!tex.LoadImage(req.downloadHandler.data))
                    {
                        Debug.LogError(BASE_URL + "/users" + ": IconLoadError");
                    }
                }
                return tex;
            }
        }
    }
}