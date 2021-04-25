using System;
using UnityEngine;
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
            public string iconUrl;
        }
        public void Handler(EventType type, string msg)
        {
            if (type == EventType.UserCreated)
            {
                // {"type":1,"data":{"userId":"ID","name":"NAME","iconURL":"https://example.com"}}
                Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(msg);

                userManager.AddUser(e.data.name, e.data.userId);
                FetchIcon(e.data.userId, e.data.iconUrl);
                return;
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
