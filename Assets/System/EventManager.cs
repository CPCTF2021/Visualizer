using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace VisualizerSystem
{
    [System.Flags]
    public enum EventType
    {
        TimeAdjuster = 0,
        UserCreated = 1,
        ProblemSolved = 2,
        RankingUpdated = 3,
    }
    [Serializable]
    public class Event<T>
    {
        public EventType type;
        public T data;
    }
    [Serializable]
    class Event
    {
        public EventType type;
    }
    public class EventManager
    {
        [DllImport("__Internal")]
        private static extern void Init(string server);
        [DllImport("__Internal")]
        private static extern string PopMessage();
        [DllImport("__Internal")]
        private static extern void Close();

        [SerializeField]
        // static string WS_URI = "wss://cpctf.space/ws/visualizer";
        static string WS_URI = "ws://localhost:3000";

        public delegate void Handler(EventType type, string msg);
        private Handler _handler;
        public void Init()
        {
            Init(WS_URI);
        }
        public void Register(Handler func)
        {
            _handler += func;
        }
        public void Handle()
        {
            var msg = PopMessage();
            if (msg != "")
            {
                try
                {
                    Event e = JsonUtility.FromJson<Event>(msg);
                    _handler(e.type, msg);
                }
                catch (ArgumentException err)
                {
                    Debug.LogError(err);
                }
            }
        }
        public void Shutdown()
        {
            Close();
        }
    }
}
