using System;
using UnityEngine;
using WebSocketSharp;
using UserScripts;
using UnityEngine.Networking;
using System.Collections;

namespace VisualizerSystem
{
    public class UserCreatedEvent : MonoBehaviour
    {
        static UserManager userManager;

        public UserCreatedEvent(UserManager manager)
        {
            userManager = manager;
        }
        public void Init(UserManager manager)
        {
            userManager = manager;
        }
        [Serializable]
        private class EventDetail
        {
            public string userId;
            public string name;
        }
        public void Handler(MessageEventArgs args)
        {
            Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
            if (e.type == EventType.UserCreated) 
            {
                StartCoroutine(AddUser(e.data.userId, e.data.name));
                return;
            }
        }
        private IEnumerator AddUser(string userId, string name)
        {
            string URL = $"https://example.com/users/{userId}/icon";
            using (UnityWebRequest req = UnityWebRequest.Get(URL))
            {
                yield return req.SendWebRequest();

                Debug.Log(req.downloadHandler.text);
                var tex = new Texture2D(2, 2);
                if (req.isHttpError || req.isNetworkError)
                {
                    Debug.LogError(URL + ": Error: " + req.error);
                }
                else
                {
                    Debug.Log("SUCCESS: " + URL);
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
