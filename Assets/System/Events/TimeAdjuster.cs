using System;
using UnityEngine;
using UserScripts;

namespace VisualizerSystem
{
    public class TimeAdjusterEvent
    {
        static UserManager userManager;
        Timer timer;
        int counter = 0;
        public TimeAdjusterEvent(UserManager manager, Timer timer)
        {
            userManager = manager;
            this.timer = timer;
        }
        [Serializable]
        private class EventDetail
        {
            public float timeLimit;
        }
        public void Handler(EventType type, string msg)
        {
            if (type == EventType.TimeAdjuster)
            {
                // {"type":0,"data":{"timeLimit": 3000}
                Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(msg);
                timer.limitTime = e.data.timeLimit;
                if (counter == 5)
                {
                    counter = 0;
                    return;
                }
                counter++;
                return;
            }
        }
    }
}