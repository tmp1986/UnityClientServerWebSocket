using EasyButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SimpleWebSocketClient : MonoBehaviour
{
    private static string staticConnectionAddress = @"ws://192.168.2.19:3333";

    public string address = staticConnectionAddress;
    public string msgToSend = "Hello from Client";
    public string msgToReceive = "";
    WebSocket ws = new WebSocket(staticConnectionAddress + "/Echo");

    public delegate void MessageTriggerClient(string msg);
    public static event MessageTriggerClient onTriggered;

    public delegate void MessageReceived(object sender, MessageEventArgs e);
    public static event MessageReceived onMessageReceived;

    [Button]
    public void StartListening()
    {
        staticConnectionAddress = address;
        ws.OnMessage += Ws_OnMessage;
        ws.OnOpen += Ws_OnOpen;
        try
        {
            ws.Connect();

#if UNITY_EDITOR
            Debug.Log("Trying to connect...");
#endif
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e.ToString());
#endif
        }
    }

    [Button]
    public void StopListening()
    {
        ws.Close();
#if UNITY_EDITOR
        Debug.Log("Stop Listener");
#endif
    }

    private void Ws_OnOpen(object sender, EventArgs e)
    {
#if UNITY_EDITOR
        Debug.Log("Connected");
#endif
    }

    [Button]
    public void SendMessageToServer()
    {
        try
        {
            ws.Send(msgToSend);
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e.ToString());
#endif
        }
      
    }
    void SendMessageToServer(string _msg)
    {
        ws.Send(_msg);
    }


    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
#if UNITY_EDITOR
        Debug.Log("received from server: " + e.Data.ToString());
#endif
        msgToReceive = e.Data.ToString();
        onTriggered.Invoke(msgToReceive);
        onMessageReceived.Invoke(sender, e);
    }


}
