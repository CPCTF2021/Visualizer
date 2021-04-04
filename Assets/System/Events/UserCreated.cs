using System;
using UnityEngine;
using WebSocketSharp;
using UserScripts;
using UnityEngine.Networking;
using RankingScript;
using System.Linq;

namespace VisualizerSystem
{
    public class UserCreatedEvent
    {
        static UserManager userManager;
        static RankingManager rankingManager;
        public UserCreatedEvent(UserManager u_manager, RankingManager r_manager)
        {
            userManager = u_manager;
            rankingManager = r_manager;
        }
        [Serializable]
        private class EventDetail
        {
            public string userId;
            public string name;
            public string iconURL;
        }
        public void Handler(MessageEventArgs args)
        {
            // テスト用
            if (args.Data == "user")
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var random = new System.Random();
                var randomId = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
                //var randomName = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
                AddUser(randomId, randomId, "https://example.com");
                return;
            }

            try {
                Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
                if (e.type == EventType.UserCreated)
                {
                    AddUser(e.data.userId, e.data.name, e.data.iconURL);
                    return;
                }
            } catch { };
        }
        private async void AddUser(string userId, string name, string iconURL)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(iconURL))
            {
                await req.SendWebRequest();

                Debug.Log(req.downloadHandler.text);
                var tex = new Texture2D(2, 2);
                if (req.isHttpError || req.isNetworkError)
                {
                    Debug.LogError(iconURL + ": Error: " + req.error);
                }
                else
                {
                    Debug.Log("SUCCESS: " + iconURL);
                    if (!tex.LoadImage(req.downloadHandler.data))
                    {
                        Debug.LogError(userId + ": IconLoadError");
                    }
                    try { userManager.AddUser(name, userId, tex); }
                    catch (MissingFieldException err) { Debug.LogError(err); }
                    try { rankingManager.AddUser(userManager.usersDictionary[userId]); }
                    catch (ArgumentException err) { Debug.LogError(err); }
                }
            }
        }
    }
}
