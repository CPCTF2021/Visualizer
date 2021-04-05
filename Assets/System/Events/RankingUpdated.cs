using System;
using UnityEngine;
using RankingScript;
using UserScripts;

namespace VisualizerSystem
{
    public class RankingUpdatedEvent
    {
        static UserManager userManager;
        static RankingManager rankingManager;
        public RankingUpdatedEvent(UserManager u_manager, RankingManager r_manager)
        {
            userManager = u_manager;
            rankingManager = r_manager;
        }
        [Serializable]
        private class EventDetail
        {
            public string userId;
            public int ranking;
        }
        public void Handler(EventType type, string msg)
        {
            if (type == EventType.RankingUpdated)
            {
                // { "type": 3, "data": { "userId": "ID", "ranking": 1 } }
                Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(msg);

                var user = userManager.usersDictionary[e.data.userId];
                try { rankingManager.Update(user); }
                catch (Exception err) { Debug.LogError(err); }
                return;
            }
        }
    }
}