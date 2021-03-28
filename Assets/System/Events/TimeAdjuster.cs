using System;
using UnityEngine;
using WebSocketSharp;
using UserScripts;

namespace VisualizerSystem
{
    public class TimeAdjusterEvent
    {
        static UserManager userManager;
        public TimeAdjusterEvent(UserManager manager)
        {
            userManager = manager;
        }
        [Serializable]
        private class EventDetail
        {
            public int timeLimit;
        }
        public void Handler(MessageEventArgs args)
        {
            //    Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
            //    if (e.type == EventType.TimeAdjuster)
            //    {

            //    }
        }
    }
}