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
            string okMessage = GetJsonOkMessage("client/connected");
            Console.WriteLine("okmessage: " + okMessage);
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
                Console.WriteLine(ReadTextMessage(tcpClient) + "fffff");
                //JObject message = JObject.Parse(ReadTextMessage(tcpClient));
                //if(message["id"].ToString() == "server/login")
                //{
                //    ClientLogin(message["data"]["patientId"].ToString());
                //}
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

        public void ClientLogin(string patientId)
        {
            Console.WriteLine(patientId);

            if (DataSaver.ClientExists(patientId))
            {
                this.patientId = patientId;
                WriteTextMessage(this.tcpClient, GetJsonLoggedinMessage(false));
            }
            else
            {
                this.patientId = patientId;
                DataSaver.AddNewClient(this);
                WriteTextMessage(this.tcpClient, GetJsonLoggedinMessage(true));
            }
        }

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

        public void SaveSession(List<JObject> sessionData)
        {


            WriteTextMessage(tcpClient, GetJsonOkMessage("client/received"));
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

        public static string ReadTextMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                return stream.ReadLine();
            }
        }
    }
}