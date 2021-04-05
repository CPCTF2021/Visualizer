using System;
using UnityEngine;
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
            public Genre genre;
        }
        [System.Flags]
        public enum Genre
        {
            Newbie = 0,
            PPC = 1,
            Web = 2,
            Crypto = 3,
            Binary = 4,
            Pwn = 5,
            Misc = 6,
            Shell = 7,
            Foresic = 8,
        }
        public void Handler(EventType type, string msg)
        {
            // { "type": 2, "data": { "userId": "ID", "point": 2000, "genre": 2 } }
            if (type == EventType.ProblemSolved)
            {
                Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(msg);

                try { userManager.AddScore(e.data.userId, e.data.genre, e.data.point); }
                catch (MissingFieldException err) { Debug.LogError(err); }
                return;
            }
        }
    }
}