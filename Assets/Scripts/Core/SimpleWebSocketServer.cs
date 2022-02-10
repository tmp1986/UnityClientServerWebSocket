using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using EasyButtons;



public class Echo : WebSocketBehavior
{
    public static Echo current;
    public Echo()
    {
        current = this;
    }

    public delegate void MessageTrigger(string msg);
    public static event MessageTrigger onTriggered; 
    
    public string msgToReceive { get; set; }

    protected override void OnMessage(MessageEventArgs e)
    {
#if UNITY_EDITOR
        Debug.Log("Received message from client: " + e.Data.ToString());
#endif
        Send(e.Data);
        msgToReceive = e.Data.ToString();
        onTriggered.Invoke(msgToReceive);
    }
}

public class SimpleWebSocketServer : MonoBehaviour
{
      

    private static string staticAddress = @"ws://192.168.2.19:3333";
    [SerializeField] //Helper
    public string address = staticAddress;
    [SerializeField]
    private bool runServerOnStart = false;
    private WebSocketServer wssv = new WebSocketServer(staticAddress);

    public string msgToReceive { get; set; }
    public string msgToSend = "Hello from Server";

    // Start is called before the first frame update
    private void Start()
    {
        staticAddress = address;
        wssv.AddWebSocketService<Echo>("/Echo");
        //Echo.onTriggered += Current_onMessageTrigger;

        if (runServerOnStart)
        {
            StartServer();
        }
    }

    //private void Current_onMessageTrigger()
    //{
    //    Debug.Log("CHEGU");
    //}


    //Start the websocket server
    [Button]
    public void StartServer()
    {
        try
        {
            wssv.Start();
#if UNITY_EDITOR
            Debug.Log("WS server started on ws: " + staticAddress + "/Echo");
#endif
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.Log("Exception: " + e);
#endif
        }
    }

    //Stop websocket server
    [Button]
    public void StopServer()
    {
        if (wssv.IsListening)
        {
            wssv.Stop();
#if UNITY_EDITOR
            Debug.Log("WS server stoped on ws: " + wssv.Address.ToString() + "/Echo");
#endif
        }
    }

    //Send message to all connected socket clients
    private void SendBroadcastMessage(string msg)
    {
        foreach (var p in wssv.WebSocketServices.Paths)
        {
            wssv.WebSocketServices[p].Sessions.Broadcast(msg);
        }
    }

 
    void OnApplicationQuit()
    {
        wssv.Stop();
#if UNITY_EDITOR
        Debug.Log("WS server stoped on ws " + staticAddress);
#endif
    }

   

    #region Interface Buttons
    [Button]
    public void SendMsgToAllClients()
    {
        SendBroadcastMessage(msgToSend);
    }

    public void SendMsgToAllClients(string msg)
    {
        SendBroadcastMessage(msg);
    }
    #endregion



}

//Use the simple web socket client for chrome to test
