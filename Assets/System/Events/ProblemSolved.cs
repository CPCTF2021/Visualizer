using System;
using UnityEngine;
using WebSocketSharp;
using UserScripts;

namespace VisualizerSystem
{
    public class ProblemSolvedEvent
    {
        static UserManager userManager;
        public ProblemSolvedEvent(UserManager manager)
        {
            userManager = manager;
        }
        [Serializable]
        private class EventDetail
        {
            public string userId;
            public int point;
            public int genre;
        }
        public void Handler(MessageEventArgs args)
        {
            Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
            if (e.type == EventType.ProblemSolved)
            {
                try { userManager.AddPoint(e.data.userId, e.data.genre, e.data.point); }
                catch (MissingFieldException err)
                {
                    Debug.LogError(err);
                }
                return;
            }
        }
    }
}
