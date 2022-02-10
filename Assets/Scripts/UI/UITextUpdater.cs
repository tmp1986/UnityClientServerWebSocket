using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Threading;
using System.Threading.Tasks;

public class UITextUpdater : MonoBehaviour
{

    [SerializeField]
    private GameObject websocket;

    [SerializeField]
    private TMP_InputField adressInputField;

    [SerializeField]
    private TMP_InputField sendMsgInputField;
    [SerializeField]
    private TextMeshProUGUI receivedMsgTMP;

    Echo echo = new Echo();

    private bool canUpdateText = false; 

    public string msgToReceive { get; set; }
 
    [SerializeField]
    private bool isserver = false;

    private void Start()
    {
        Echo.onTriggered += ReceivedMessageTrigger; ;

        SimpleWebSocketClient.onTriggered += ReceivedMessageTrigger;
    }

  

    void Update()
    {
        if (canUpdateText)
        {
            receivedMsgTMP.text = msgToReceive;
            canUpdateText = false;
        }
        
    }

    private void ReceivedMessageTrigger(string msg)
    {
        //string msg = echo.();
        Debug.Log(msg);
        msgToReceive = msg;
        canUpdateText = true;
    }



    public void UpdateTextOnUI()
    {
        if (isserver)
            websocket.GetComponent<SimpleWebSocketServer>().msgToSend = sendMsgInputField.text;
        else
            websocket.GetComponent<SimpleWebSocketClient>().msgToSend = sendMsgInputField.text;
    }

    public void UpdateClientServerAdressOnChange()
    {
        if (isserver)
            websocket.GetComponent<SimpleWebSocketServer>().address = adressInputField.text;
        else
            websocket.GetComponent<SimpleWebSocketClient>().address = adressInputField.text;
    }




}
