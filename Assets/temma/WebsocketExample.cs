using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebsocketExample : MonoBehaviour
{
    WebSocket ws;
    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket ("ws://localhost:3000"); // $ npx wscat -l 3000
        ws.EmitOnPing = true;
        ws.OnMessage += (sender, e) =>
            Debug.Log("Laputa says: " + e.Data);

        ws.Connect();
        ws.Send("BALUS");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}
