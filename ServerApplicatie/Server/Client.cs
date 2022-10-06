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

        public string receivedJsonMessage = "";

        public class Patient
        {
            public string patientId { get; set; }
        }

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            string okMessage = JsonMessageGenerator.GetJsonOkMessage("server/connected");
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
                if (loginRequest["id"].ToString() == "doctor/login")
                {
                    if (loginRequest["username"].ToString() == "dokter" && loginRequest["password"].ToString() == "wachtwoord")
                    {
                        string loginConfirm = JsonMessageGenerator.GetJsonOkMessage("server/login");
                        
                        WriteTextMessage(tcpClient, loginConfirm + "\n");

                        while (true)
                        {
                            JObject jsonSessionData = JObject.Parse(ReadJsonMessage(tcpClient));
                            Console.WriteLine(jsonSessionData.ToString());
                            if (jsonSessionData["id"].ToString() == "doctor/clients")
                            {
                                List<Patient> patients = new List<Patient>();
                                JToken clients = jsonSessionData["data"];
                                
                                foreach(var client in clients)
                                {
                                    patients.Add(new Patient() { patientId = client.ToString()});
                                }
                            }
                            else if(jsonSessionData["id"].ToString() == "doctor/startSession")
                            {
                                string patientId = jsonSessionData["client"].ToString();

                                for(int i = 0; i < Program.clients.Count; i++)
                                {
                                    if (patientId == Program.clients[i].patientId)
                                    {
                                        Program.clients[i].receivedJsonMessage = JsonMessageGenerator.GetJsonStartSessionMessage("server/" + patientId);
                                    }
                                }
                            }


                            /*
                            Console.WriteLine(jsonSessionData);
                            JObject jsonMessage = JObject.Parse(jsonSessionData);
                            if ((bool)jsonMessage["data"]["endOfSession"])
                            {
                                sessionData.Add(jsonMessage);
                                SaveSession(sessionData);
                                sessionData.RemoveRange(0, sessionData.Count);
                                WriteJsonMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("server/received"));

                            }
                            else
                            {
                                sessionData.Add(jsonMessage);
                            }
                            */
                        }

                    }
                }
                else if (loginRequest["id"].ToString() == "client/login")
                {
                    if (DataSaver.ClientExists(loginRequest["data"]["patientId"].ToString()))
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
                    while (true)
                    {
                        while(receivedJsonMessage == "")
                        {
                            
                        }
                        

                        /*
                        string jsonSessionData = ReadJsonMessage(tcpClient);
                        Console.WriteLine(jsonSessionData);
                        JObject jsonMessage = JObject.Parse(jsonSessionData);
                        if ((bool)jsonMessage["data"]["endOfSession"])
                        {
                            sessionData.Add(jsonMessage);
                            SaveSession(sessionData);
                            sessionData.RemoveRange(0, sessionData.Count);
                            WriteJsonMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("server/received"));
                        }
                        else
                        {
                            sessionData.Add(jsonMessage);
                        }
                        */
                    }
                }
                
            }
            
        }

        public void SaveSession(List<JObject> sessionData)
        {


            WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("server/received"));
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