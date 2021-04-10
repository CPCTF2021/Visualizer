using System;
using UnityEngine;
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
        public void Handler(EventType type, string msg)
        {
            //    Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
            //    if (e.type == EventType.TimeAdjuster)
            //    {

            //    }
        }
    }
}