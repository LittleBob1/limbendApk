using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using System.IO;
using System.Collections;

namespace ServerCommands {

    public class ServerInitialization : MonoBehaviour
    {

        TMP_Text TextConsole;

        private TcpListener listener = null;
        private List<TcpClient> clients;
        private NetworkStream ns = null;
        string msg;

        StreamWriter theWriter = null;

        public void StartListening(int port)
        {
            clients = new List<TcpClient>();
            IpServer i = new IpServer();
            IPAddress ip = i.GetLocalIPAddress();

            TextConsole = GameObject.Find("TextConsole").GetComponent<TMP_Text>();
            
            listener = new TcpListener(ip, port);
            listener.Start();

            TextConsole.text += "\n\nMultiplayer Server | ver. 0.1 beta\nip: " + ip + "\nport: " + port.ToString();
            TextConsole.text += "\nServer starting... Waiting for connections";            

            if (listener.Pending())
            {
                TcpClient client = listener.AcceptTcpClient();
                clients.Add(client);
                TextConsole.text += "\nConnected";
            }
        }

        public void UpdateListening()
        {
            if (listener != null)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    clients.Add(client);
                    TextConsole.text += "\n" + client.Client.RemoteEndPoint + " connected";
                }
               
                try
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        ns = clients[i].GetStream();
                        if (ns != null && ns.DataAvailable)
                        {
                            BinaryReader reader = new BinaryReader(ns);
                            msg = reader.ReadString(); 
                            TextConsole.text += "\n" + msg;

                            for (int j = 0; j < clients.Count; j++)
                            {
                                if (clients[i] != clients[j])
                                {
                                    BinaryWriter writer = new BinaryWriter(ns);
                                    writer.Write(msg);
                                    writer.Flush();
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    TextConsole.text += "\n" + e;
                }

            }
        }

        void Update()
        {
            UpdateListening();
        }

        private void OnApplicationQuit()
        {
            if (listener != null)
                listener.Stop();
        }
    }

    public class IpServer
    {

        public IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        public IPAddress GetMyIp()
        {
            List<string> services = new List<string>()
        {
            "https://ipv4.icanhazip.com",
            "https://api.ipify.org",
            "https://ipinfo.io/ip",
            "https://checkip.amazonaws.com",
            "https://wtfismyip.com/text",
            "http://icanhazip.com"
        };
            using (var webclient = new WebClient())
                foreach (var service in services)
                {
                    try { return IPAddress.Parse(webclient.DownloadString(service)); } catch { }
                }
            return null;
        }

        public IPAddress GetMyIpWithHttp()
        {
            List<string> services = new List<string>()
        {
            "https://ipv4.icanhazip.com",
            "https://api.ipify.org",
            "https://ipinfo.io/ip",
            "https://checkip.amazonaws.com",
            "https://wtfismyip.com/text",
            "http://icanhazip.com"
        };
            using (var httpClient = new HttpClient())
                foreach (var service in services)
                {
                    try { return IPAddress.Parse(httpClient.GetStringAsync(service).Result); } catch { }
                }
            return null;
        }
    }
}

