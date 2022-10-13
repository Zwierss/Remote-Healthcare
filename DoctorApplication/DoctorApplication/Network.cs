using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DoctorApplication
{
    public class Network
    {
        private static TcpClient tcpClient;

        private string username = "";
        private string password = "";

        public delegate void ContinueLogin();
        public ContinueLogin login;

        public delegate void HandleSessionData();
        public HandleSessionData showSessionData;

        //public string command = "";
        public JObject jCommand = new JObject();

        public JObject sessionData = new JObject();

        public DoctorMainPage main;


        public Network(string username, string password, ContinueLogin login)
        {
            this.username = username;
            this.password = password;
            this.login = login;
        }

        public void StartConnection()
        {
            tcpClient = new TcpClient();
            tcpClient.BeginConnect("192.168.43.137", 15243, new AsyncCallback(OnConnect), null);

        }

        private void OnConnect(IAsyncResult ar)
        {
            tcpClient.EndConnect(ar);
            Console.WriteLine("Verbonden!");
            HandlingServer();
        }

        public async void HandlingServer()
        {
            JObject connectedMessage = JObject.Parse(ReadJsonMessage(tcpClient));
            if (connectedMessage["status"].ToString() == "ok")
            {
                WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonLoginMessage(username, password) + "\n");
            }

            JObject loginReply = JObject.Parse(ReadJsonMessage(tcpClient));
            if (loginReply["status"].ToString() == "ok")
            {
                login();
                while (true)
                {
                    while(this.jCommand.ToString() == new JObject().ToString())
                    {

                    }
                    WriteTextMessage(tcpClient, this.jCommand + "\n");

                    if (jCommand["id"].ToString() == "doctor/stopSession")
                    {
                        jCommand = new JObject();
                    }
                    else
                    {
                        while (jCommand["id"].ToString() == "doctor/startSession")
                        {
                            Trace.WriteLine("doctor startsession");
                            WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("doctor/received") + "\n");
                            string text = ReadJsonMessage(tcpClient);
                            Trace.WriteLine("Text: " + text);
                            JObject sessionData = JObject.Parse(text);
                            main.sessionData = sessionData.ToString();
                            this.showSessionData();
                        }
                    }

                    //jCommand = new JObject();
                }
            }
            else
            {
                login();
                Thread.CurrentThread.Abort();
            }
        }

        public void returnCommand(JObject jCommand)
        {
            this.jCommand = jCommand;
        }

        public static string ReadJsonMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                string message = "";
                while (stream.Peek() != -1)
                {
                    message += stream.ReadLine();
                }
                return message;
            }
        }

        public static void WriteTextMessage(TcpClient client, string message)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            {
                stream.Write(message);
                stream.Flush();
            }
        }

    }
}
