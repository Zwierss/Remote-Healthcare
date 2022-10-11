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
            WriteMessage(this.tcpClient, JsonMessageGenerator.GetJsonOkMessage("client/connected"));
            Thread thread = new Thread(HandleClient);
            thread.Start();
        }
        
        public void HandleClient()
        {
            bool loggedIn = false;
            while (!loggedIn)
            {
                JObject loginRequest = ReadMessage(tcpClient);
                string patientId = loginRequest["data"]["patientId"].ToString();
                if(DataSaver.ClientExists(patientId))
                {
                    this.patientId = patientId;
                    WriteMessage(tcpClient, JsonMessageGenerator.GetJsonLoggedInMessage(false));
                }
                else
                {
                    this.patientId = patientId;
                    DataSaver.AddNewClient(this);
                    WriteMessage(tcpClient, JsonMessageGenerator.GetJsonLoggedInMessage(true));
                }
            }
            while (true)
            {
                JObject jsonMessage = ReadMessage(tcpClient);
                Console.WriteLine(jsonMessage.ToString());
                if ((bool) jsonMessage["data"]["endOfSession"])
                {
                    sessionData.Add(jsonMessage);
                    SaveSession(sessionData);
                    sessionData.RemoveRange(0, sessionData.Count);
                    WriteMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("client/received"));
                }
                else
                {
                    sessionData.Add(jsonMessage);
                }
            }
        }

        public void SaveSession(List<JObject> sessionData)
        {
            WriteMessage(tcpClient,JsonMessageGenerator.GetJsonOkMessage("client/received"));
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

        public static JObject ReadMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                string message = "";
                string line = "";
                while (stream.Peek() != -1)
                {
                    message += stream.ReadLine();
                }
                
                return JObject.Parse(message);
            }
        }
    }
}