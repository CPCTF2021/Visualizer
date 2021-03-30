using System;
using UnityEngine;
using WebSocketSharp;
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
        public void Handler(MessageEventArgs args)
        {
            Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
            if (e.type == EventType.RankingUpdated)
            {
                var before = userManager.usersDictionary[e.data.userId];
                var after = new User();
                after.SetUser(before.name, before.id, before.icon, before.scores, e.data.ranking);
                try { rankingManager.Update(before, after); }
                catch (ArgumentException err) { Debug.LogError(err); }
                return;
            }
        }
    }
}