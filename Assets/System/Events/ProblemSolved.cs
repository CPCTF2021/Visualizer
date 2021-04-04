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
        public void Handler(MessageEventArgs args)
        {
            // テスト用: [point id genre score]
            if (args.Data.StartsWith("point"))
            {
                var data = args.Data.Split(' ');
                var genre = (Genre)int.Parse(data[2]);
                var score = float.Parse(data[3]);
                try { userManager.AddScore(data[1], genre, score); }
                catch (MissingFieldException err) { Debug.LogError(err); }
                return;
            }
            try
            {
                Event<EventDetail> e = JsonUtility.FromJson<Event<EventDetail>>(args.Data);
                if (e.type == EventType.ProblemSolved)
                {
                    try { userManager.AddScore(e.data.userId, e.data.genre, e.data.point); }
                    catch (MissingFieldException err) { Debug.LogError(err); }
                    return;
                }
            } catch { };
        }
    }
}
