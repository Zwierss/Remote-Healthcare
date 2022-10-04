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
    class Network
    {
        private static TcpClient tcpClient;

        private string username = "";
        private string password = "";

        public delegate void ContinueLogin(bool succeeded);
        public ContinueLogin login;

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

        public void HandlingServer()
        {
            //bool loggedIn = false;

            JObject connectedMessage = JObject.Parse(ReadJsonMessage(tcpClient));
            if (connectedMessage["status"].ToString() == "ok")
            {
                //WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonLoginMessage(username, password));
                WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonLoginMessage(username, password));
            }

            JObject loginReply = JObject.Parse(ReadJsonMessage(tcpClient));
            if (loginReply["status"].ToString() == "ok")
            {
                //loggedIn = true;
                login(true);
                
                while (true)
                {
                    try
                    {
                        JObject command = JObject.Parse(Console.ReadLine());
                        WriteTextMessage(tcpClient, command.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }

                }
            }
            else
            {
                login(false);
                Thread.CurrentThread.Abort();
            }
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
