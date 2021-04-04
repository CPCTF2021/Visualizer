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
            // テスト用: [ranking userId ranking]
            if (args.Data.StartsWith("ranking"))
            {
                var data = args.Data.Split(' ');
                var user = userManager.usersDictionary[data[1]];
                user.SetRanking(int.Parse(data[2]));
                try { rankingManager.Update(user); }
                catch (Exception err) { Debug.LogError(err); }
                return;
            }

            try
            {
                Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
                if (e.type == EventType.RankingUpdated)
                {
                    var user = userManager.usersDictionary[e.data.userId];
                    try { rankingManager.Update(user); }
                    catch (Exception err) { Debug.LogError(err); }
                    return;
                }
            }
            catch { };
        }
    }
}