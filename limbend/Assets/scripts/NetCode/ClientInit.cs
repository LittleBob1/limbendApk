using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System;
using System.Text;
using TMPro;

public class ClientInit : MonoBehaviour
{
    TMP_Text TextConsole;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;
    TcpClient mySocket = null;
    //TcpClient server = null;
    private string msg;

    IPAddress Host;
    int Port;

    private int a = 0;

    public bool SendMsgFromClient(string msg)
    {
        try
        {
            theStream = mySocket.GetStream();
            BinaryWriter writer = new BinaryWriter(theStream);
            writer.Write(mySocket.Client.LocalEndPoint + ": " + msg);
            writer.Flush();
            return true;
        }
        catch (Exception e)
        {
            mySocket = null;
            TextConsole.text += "\nSocket error: " + e;
            return false;
        }
    }

    public void CreateClient(IPAddress ip, int port)
    {
        StartCoroutine(CheckConnect());

        TextConsole = GameObject.Find("TextConsole").GetComponent<TMP_Text>();

        Host = ip;
        Port = port;

        mySocket = new TcpClient();
        mySocket.ConnectAsync(Host, Port);

        if (mySocket.Connected)
        {
            if (SendMsgFromClient(mySocket.Client.LocalEndPoint.ToString()))
            {
                TextConsole.text += "\nConnected";
            }
        }
    }

    private void ReceivingMsgs()
    {
        if (mySocket != null)
        {
            if (mySocket.Connected)
            {
                theStream = mySocket.GetStream();
                if (theStream.DataAvailable)
                {
                    BinaryReader reader = new BinaryReader(theStream);
                    msg = reader.ReadString();
                    TextConsole.text += "\n" + msg;
                }
            }
        }
    }

    private void Update()
    {
        if (mySocket != null)
        {
            if (mySocket.Connected)
            {
                if (a == 0)
                {
                    a++;
                    TextConsole.text += "\nConnected: " + mySocket.Connected;
                    SendMsgFromClient("Hello from " + mySocket.Client.LocalEndPoint.ToString());
                }
            }
        }
        ReceivingMsgs();
    }

    IEnumerator CheckConnect()
    {
        yield return new WaitForSeconds(1f);
        if(mySocket != null)
        {
            Debug.Log(mySocket.Connected);
        }
        StartCoroutine(CheckConnect());
    }

    private void OnApplicationQuit()
    {
        if (mySocket != null && mySocket.Connected)
            mySocket.Close();
    }
}
