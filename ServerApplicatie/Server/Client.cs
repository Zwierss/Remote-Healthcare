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
            WriteTextMessage(this.tcpClient, okMessage + "\r\n");
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
                string patientId = loginRequest["data"]["patientId"].ToString();
                if(DataSaver.ClientExists(patientId))
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
                string jsonSessionData = ReadJsonMessage(tcpClient);
                Console.WriteLine(jsonSessionData);
                JObject jsonMessage = JObject.Parse(jsonSessionData);
                if ((bool) jsonMessage["data"]["endOfSession"])
                {
                    sessionData.Add(jsonMessage);
                    SaveSession(sessionData);
                    sessionData.RemoveRange(0, sessionData.Count);
                    WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("client/received"));
                }
                else
                {
                    sessionData.Add(jsonMessage);
                }
            }
        }

        public void SaveSession(List<JObject> sessionData)
        {
            WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("client/received"));
            DataSaver.AddPatientFile(tcpClient, sessionData);
        }

        public static void WriteMessage(TcpClient client, JObject jObject)
        {
            string jMessage = jObject.ToString();
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            byte[] RequestLength = BitConverter.GetBytes(jMessage.Length);
            byte[] request = Encoding.ASCII.GetBytes(jMessage);
            {
                stream.BaseStream.Write(request, 0, RequestLength.Length);
            }
        }

        public static string ReadMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                return stream.ReadLine();
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
                
                return message;
            }
        }
    }
}