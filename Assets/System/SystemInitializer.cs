using UnityEngine;
using UserScripts;
using TreeScripts;
using static TreeScripts.ControlTree;
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
        // leaveの色変換は一時的なもの
        [SerializeField]
        Color leaveBaseColor;
        [SerializeField]
        float baseColorRate = 0.5f;
        static UserManager userManager;
        static EventManager eventManager = new EventManager();
        static RankingManager rankingManager;
        static string BASE_URL = "https://cpctf.space";
        void Start()
        {
            // 葉の色
            for (int i = 0; i < GENRE_TO_COLOR.Length; i++)
            {
                GENRE_TO_COLOR[i] = GENRE_TO_COLOR[i] * 0.95f + new Color(1f, 1f, 1f) * 0.05f;
                GENRE_TO_COLOR[i] = GENRE_TO_COLOR[i] * baseColorRate + leaveBaseColor * (1f - baseColorRate);
                // Points.GENRE_TO_COLOR[i] = Mathf.Pow(new Color(1f, 1f, 1f) - Points.GENRE_TO_COLOR[i], 2.0f);
            }
            GetComponent<TreeGenerator>().MakeTree();
            userManager = GetComponent<UserManager>();
            userManager.SetTree();

            Sync();

            eventManager.Init();

            rankingManager = GameObject.Find("Scroll View").GetComponent<RankingManager>();

            TimeAdjusterEvent timeAdjusterEvent = new TimeAdjusterEvent(userManager);
            eventManager.Register(timeAdjusterEvent.Handler);

            UserCreatedEvent userCreatedEvent = new UserCreatedEvent(userManager, rankingManager);
            eventManager.Register(userCreatedEvent.Handler);

            ProblemSolvedEvent problemSolvedEvent = new ProblemSolvedEvent(userManager);
            eventManager.Register(problemSolvedEvent.Handler);
        }
        void Update()
        {
                eventManager.Handle();
        }
        void OnDestroy()
        {
            eventManager.Shutdown();
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
        private class Usera : User
        {
            public string id;
            public string name;
            public Texture2D icon;
            Dictionary<Genre, float> scores;
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