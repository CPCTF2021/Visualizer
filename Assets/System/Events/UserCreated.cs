using System;
using UnityEngine;
using WebSocketSharp;
using UserScripts;
using UnityEngine.Networking;
using RankingScript;

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
            Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
            if (e.type == EventType.UserCreated) 
            {
                AddUser(e.data.userId, e.data.name, e.data.iconURL);
                try { rankingManager.AddUser(userManager.usersDictionary[e.data.userId]); }
                catch (ArgumentException err) { Debug.LogError(err); }
                return;
            }
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
                    catch (MissingFieldException err)
                    {
                        Debug.LogError(err);
                    }
                }
            }
        }
    }
}
