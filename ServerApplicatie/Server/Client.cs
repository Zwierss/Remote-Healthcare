using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DataSaving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    internal class Client
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private string totalBuffer = "";

        public string patientId { get; set; }

        List<JObject> sessionData = new List<JObject>();


        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            string okMessage = JsonMessageGenerator.GetJsonOkMessage("client/connected");
            WriteJsonMessage(this.tcpClient, okMessage + "\r\n");
            Thread thread = new Thread(HandleClient);
            thread.Start();
        }
        
        public Client()
        {
            this.tcpClient = null;

        }

        public void HandleClient()
        {
            bool loggedIn = false;
            while (!loggedIn)
            {
                JObject loginRequest = JObject.Parse(ReadJsonMessage(tcpClient));
                if(DataSaver.ClientExists(loginRequest["data"]["patientId"].ToString()))
                {
                    this.patientId = patientId;
                    WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonLoggedinMessage(false) + "\n");
                }
                else
                {
                    this.patientId = patientId;
                    DataSaver.AddNewClient(this);
                    WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonLoggedinMessage(true) + "\n");
                }
            }
            while (true)
            {
                string message = ReadTextMessage(tcpClient);
                JObject jsonMessage = JObject.Parse(message);
                if ((bool) jsonMessage["data"]["endOfSession"])
                {
                    sessionData.Add(jsonMessage);
                    SaveSession(sessionData);
                    sessionData.RemoveRange(0, sessionData.Count);
                }
                else
                {
                    sessionData.Add(jsonMessage);
                }
            }
        }

        /*
        public string GetJsonOkMessage(string id)
        {
            JObject message = new JObject();
            message.Add("id", id);
            message.Add("status", "ok");

            return message.ToString();
        }

        public string GetJsonLoggedinMessage(bool newAccount)
        {
            JObject message = new JObject();
            message.Add("id", "client/login");
            message.Add("newAccount", newAccount);

            return message.ToString();
        }
        */

        public void SaveSession(List<JObject> sessionData)
        {


            WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("client/received"));
        }

        public void HandleData()
        {
            bool connected = true;
            while (connected)
            {
                string received = ReadTextMessage(this.tcpClient);
            }
        }

        public static void WriteJsonMessage(TcpClient client, string jsonMessage)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            {
                stream.Write(jsonMessage);
                stream.Flush();
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

        public static string ReadJsonMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                string message = "";
                string line = "";
                while (stream.Peek() != -1)
                {
                    message += stream.ReadLine();
                }
                Console.WriteLine(message);
                return message;
            }
        }

        public static string ReadTextMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                return stream.ReadLine();
            }
        }
    }
}