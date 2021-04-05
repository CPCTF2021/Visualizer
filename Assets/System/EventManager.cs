using UnityEngine;
using HybridWebSocket;
using System;
using System.Collections.Concurrent;

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
        [SerializeField]
        static string WS_URI = "ws://localhost:3000";
        WebSocket ws;
        public ConcurrentQueue<byte[]> incoming_messages = new ConcurrentQueue<byte[]>();
        public delegate void Handler(EventType type, string msg);
        private Handler _handler;
        public void Init()
        {

            ws = WebSocketFactory.CreateInstance(WS_URI);
            ws.OnMessage += (e) =>
            {
                incoming_messages.Enqueue(e);
            };
            ws.Connect();
        }
        public void Register(Handler func)
        {
            _handler += func;
        }
        public void Handle(byte[] msg)
        {
            try
            {
                string json = System.Text.Encoding.UTF8.GetString(msg);
                Event e = JsonUtility.FromJson<Event>(json);
                _handler(e.type, json);
            }
            catch (ArgumentException err)
            {
                Debug.LogError(err);
            }
        }
        public void Shutdown()
        {
            ws.Close();
            ws = null;
        }
    }
}
