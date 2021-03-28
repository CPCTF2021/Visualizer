using System;
using UnityEngine;
using WebSocketSharp;
using UserScripts;

namespace VisualizerSystem
{
    public class RankingUpdatedEvent
    {
        static UserManager userManager;
        public RankingUpdatedEvent(UserManager manager)
        {
            userManager = manager;
        }
        [Serializable]
        private class EventDetail
        {
            public string userId;
            public int ranking;
        }
        public void Handler (MessageEventArgs args)
        {
            //Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
            //if (e.type == EventType.RankingUpdated)
            //{

            //}
        }
    }
}