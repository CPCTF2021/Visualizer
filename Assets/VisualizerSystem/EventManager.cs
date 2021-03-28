using UnityEngine;
using WebSocketSharp;
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
    public class EventManager
    {
        [SerializeField]
        static string WS_URI = "ws://localhost:3000";
        WebSocket ws;
        public ConcurrentQueue<MessageEventArgs> incoming_messages = new ConcurrentQueue<MessageEventArgs>();
        public delegate void Handler(MessageEventArgs e);
        private Handler _handler;
        public void Init()
        {
            ws = new WebSocket(WS_URI);
            ws.EmitOnPing = true;
            ws.OnMessage += (sender, e) =>
            {
                incoming_messages.Enqueue(e);
            };
            ws.Connect();
        }
        public void Register(Handler func)
        {
            _handler += func;
        }
        public void Handle(MessageEventArgs msg)
        {
            _handler(msg);
        }
        public void Shutdown()
        {
            ws.Close();
            ws = null;
        }
    }
}
