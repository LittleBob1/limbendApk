using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ServerCommands;
using System.Net;
using System;

public class CommandHandler : MonoBehaviour
{
    public GameObject Console;
    public TMP_InputField Input;
    public GameObject Enter;
    public TMP_Text ConsoleText;

    public void ClickOC()
    {
        if(Console.activeSelf == false)
        {
            Console.SetActive(true);
            Input.gameObject.SetActive(true);
            Enter.SetActive(true);
        }
        else
        {
            Console.SetActive(false);
            Input.gameObject.SetActive(false);
            Enter.SetActive(false);
        }
    }

    
    IEnumerator ddd()
    {
        while (true)
        {
            int a = 1;
            a++;
            yield return null;
        }
    }
    

    public void EnterClick()
    {

        string[] commands = Input.text.Split(" ");

        if (Input.text == "help")
        {
            ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n\nCommand List:\n<help>\n<ip>\n<position (object name)>\n<setposition (x) (y) (object name)>\n<FPS 000>\n<speed 000>\n<drag 000>\n<host server port1 00000 port2 00000>\n<active true\\false (object name)>\n";
            Input.text = null;
        }
        else if (commands[0] == "InMap")
        {
            try
            {
                GameObject.Find("MiniMapCamera").GetComponent<CameraControl>().InMap = bool.Parse(commands[1]);
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Wrong bool parametr\n";
            }
            Input.text = null;
        }
        else if (commands[0] == "setposition")
        {
            try
            {
                GameObject.Find(commands[3]).transform.position = new Vector2(float.Parse(commands[1]), float.Parse(commands[2]));
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Wrong number or name\n";
            }

            Input.text = null;
        }
        else if (commands[0] == "host")
        {
            ServerInitialization s = GameObject.Find("ServerInit").GetComponent<ServerInitialization>();
            try
            {
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
                s.StartListening(int.Parse(commands[3]));
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Wrong port\n";
            }
            Input.text = null;
        }
        else if (commands[0] == "send")
        {
            ClientInit s = GameObject.Find("ServerInit").GetComponent<ClientInit>();
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
                s.SendMsgFromClient(commands[1]);
            Input.text = null;
        }
        else if (commands[0] == "connect")
        {
            ClientInit s = GameObject.Find("ServerInit").GetComponent<ClientInit>();
            try
            {
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
                s.CreateClient(IPAddress.Parse(commands[1]), int.Parse(commands[2]));
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Wrong port or ip\n";
            }
            Input.text = null;
        }
        else if (Input.text == "ip")
        {
            try
            {
                string ips = "";
                IpServer g = new IpServer();
                string hostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
                IPAddress[] ipAdresses = ipEntry.AddressList;
                for(int i = 0; i < ipAdresses.Length; i++)
                {
                    ips += ipAdresses[i] + "\n";
                }
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n\n1. " + g.GetMyIpWithHttp() + "\n2. " + ips + "\n";
            }
            catch(Exception e)
            {
                ConsoleText.text += "\n" + e.Message;
            }
            Input.text = null;
        }
        else if (commands[0] == "FPS")
        {
            try
            {
                Application.targetFrameRate = int.Parse(commands[1]);
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Wrong number\n";
            }
            Input.text = null;
        }
        else if (commands[0] == "speed")
        {
            try
            {
                GameObject.Find("player").GetComponent<PlayerController>().speed = float.Parse(commands[1]);
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Wrong number\n";
            }
            Input.text = null;
        }
        else if (commands[0] == "drag")
        {
            try
            {
                GameObject.Find("player").GetComponent<PlayerController>().drag = float.Parse(commands[1]);
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + "\n";
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Wrong number\n";
            }
            Input.text = null;
        }
        else if (commands[0] == "position")
        {
            try
            {
                ConsoleText.text = "\n" + ConsoleText.text + "\n" + Input.text + (GameObject.Find(commands[1]).transform.position).ToString() + "\n";
            }
            catch
            {
                ConsoleText.text = "\n" + ConsoleText.text + "Invalid name\n";
            }
            Input.text = null;
        }
        else
        {
            ConsoleText.text = ConsoleText.text + "\nUnrecognized\n";
        }
    }
}
