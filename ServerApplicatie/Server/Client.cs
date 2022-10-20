using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DataSaving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Client
    {
        public TcpClient tcpClient;
        public NetworkStream stream;
        public byte[] buffer = new byte[1024];
        public string totalBuffer = "";

        public string patientId { get; set; }

        List<JObject> sessionData = new List<JObject>();

        public JObject receivedJsonMessage = new JObject();

        public JObject receivedJsonData = new JObject();

        public class Patient
        {
            public string patientId { get; set; }
        }

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            string okMessage = JsonMessageGenerator.GetJsonOkMessage("server/connected");
            WriteJsonMessage(this.tcpClient, okMessage + "\r\n");
            Thread thread = new Thread(HandleConnection);
            thread.Start();
        }
        
        public void HandleClient()
        {
            this.tcpClient = null;

        }

        public void HandleConnection()
        {
            
            bool loggedIn = false;
            while (!loggedIn)
            {
                JObject loginRequest = JObject.Parse(ReadJsonMessage(tcpClient));
                if (loginRequest["id"].ToString() == "doctor/login")
                {
                    if (loginRequest["username"].ToString() == "dokter" && loginRequest["password"].ToString() == "wachtwoord")
                    {
                        this.patientId = "Dokter";
                        string loginConfirm = JsonMessageGenerator.GetJsonOkMessage("server/login");
                        
                        WriteJsonMessage(tcpClient, loginConfirm + "\n");

                        HandleDoctor();

                    }
                }
                else if (loginRequest["id"].ToString() == "client/login")
                {
                    if (DataSaver.ClientExists(loginRequest["data"]["patientId"].ToString()))
                    {
                        this.patientId = loginRequest["data"]["patientId"].ToString();
                        WriteJsonMessage(tcpClient, JsonMessageGenerator.GetJsonLoggedinMessage(false) + "\n");
                    }
                    else
                    {
                        this.patientId = loginRequest["data"]["patientId"].ToString();
                        DataSaver.AddNewClient(this);
                        WriteJsonMessage(tcpClient, JsonMessageGenerator.GetJsonLoggedinMessage(true) + "\n");
                    }

                    HandlePatient();
                }
            }
        }

        public void HandlePatient()
        {
            while (true)
            {
                while (receivedJsonMessage.ToString() == new JObject().ToString())
                {

                }
                WriteJsonMessage(tcpClient, receivedJsonMessage + "\n");
                
                if (receivedJsonMessage["id"].ToString() == "server/startSession")
                {
                    string patientId = receivedJsonMessage["client"].ToString();
                    List<JObject> patientSessionData = new List<JObject>();
                    while (receivedJsonMessage["id"].ToString() == "server/startSession")
                    {
                        string messageToRead = ReadJsonMessage(tcpClient);
                        Console.WriteLine("Message to read" + messageToRead);
                        JObject receivedSessionData = JObject.Parse(messageToRead);

                        patientSessionData.Add(receivedSessionData);
                        for(int i = 0; i < Program.clients.Count; i++)
                        {
                            if (Program.clients[i].patientId == "Dokter")
                            {
                                Program.clients[i].sessionData = patientSessionData;
                            }
                            /*
                            if(patientId == Program.clients[i].patientId)
                            {
                                Program.clients[i].sessionData = patientSessionData;
                            }
                            */
                        }
                    }

                    /*
                    //string startSessionMessage = receivedJsonMessage;
                    List<JObject> sessionMessages = new List<JObject>();
                    while (receivedJsonMessage["id"].ToString() == "server/startSession")
                    {
                        JObject sessionMessage = JObject.Parse(ReadJsonMessage(tcpClient));
                        JObject sessionMessageToDoctor = sessionMessage;
                        sessionMessageToDoctor["id"] = "server/received";

                        for (int i = 0; i < Program.clients.Count; i++)
                        {
                            if (Program.clients[i].patientId == "")
                            {
                                Program.clients[i].receivedJsonMessage = sessionMessageToDoctor;
                            }
                        }

                        if ((bool)sessionMessage["data"]["endOfSession"] == true)
                        {
                            receivedJsonMessage["id"] = "server/stopSession";
                        }
                    }
                    */
                }
                else if (receivedJsonMessage["id"].ToString() == "server/stopSession")
                {
                    string patientId = receivedJsonMessage["client"].ToString();
                    WriteJsonMessage(tcpClient, JsonMessageGenerator.GetJsonStopSessionMessage(patientId) + "\n");
                    receivedJsonMessage = new JObject();
                }
                //receivedJsonMessage = new JObject();
            }
        }

        public void HandleDoctor()
        {
            JObject dokterMessage = JObject.Parse(ReadJsonMessage(tcpClient));

            while (true)
            {
                
                //JObject dokterMessage = JObject.Parse(ReadJsonMessage(tcpClient));

                if (dokterMessage["id"].ToString() == "doctor/clients")
                {
                    List<Patient> patients = new List<Patient>();
                    JToken clients = dokterMessage["data"];

                    foreach (var client in clients)
                    {
                        patients.Add(new Patient() { patientId = client.ToString() });
                    }
                }
                else if (dokterMessage["id"].ToString() == "doctor/startSession")
                {
                    string patientId = dokterMessage["client"].ToString();

                    for(int i = 0; i < Program.clients.Count; i++)
                    {
                        if(patientId == Program.clients[i].patientId)
                        {
                            dokterMessage["id"] = "server/startSession";
                            Program.clients[i].receivedJsonMessage = dokterMessage;
                        }
                    }

                    while (dokterMessage["id"].ToString() == "server/startSession")
                    {
                        JObject message = JObject.Parse(ReadJsonMessage(tcpClient));
                        if (message["id"].ToString() == "doctor/received")
                        {
                            Trace.WriteLine("if statement: " + this.sessionData.Count);
                            while(this.sessionData.Count == 0)
                            {

                            }
                            if (this.sessionData.Count > 0)
                            {
                                Trace.WriteLine("Session data: " + this.sessionData[this.sessionData.Count - 1]);
                                WriteJsonMessage(tcpClient, this.sessionData[this.sessionData.Count - 1].ToString() + "\n");
                            }
                        }
                        else
                        {
                            dokterMessage = message;
                        }
                        
                    }

                    /*
                    receivedJsonMessage = dokterMessage;
                    string patientId = dokterMessage["client"].ToString();

                    for (int i = 0; i < Program.clients.Count; i++)
                    {
                        if (patientId == Program.clients[i].patientId)
                        {
                            Program.clients[i].receivedJsonMessage = JObject.Parse(JsonMessageGenerator.GetJsonStartSessionMessage(patientId));
                        }
                    }
                    while (receivedJsonMessage["id"].ToString() == "doctor/startSession")
                    {
                        sessionData.Add(receivedJsonData);
                    }
                    */


                }
                else if (dokterMessage["id"].ToString() == "doctor/stopSession")
                {
                    string patientId = dokterMessage["client"].ToString();

                    for (int i = 0; i < Program.clients.Count; i++)
                    {
                        if (patientId == Program.clients[i].patientId)
                        {
                            Program.clients[i].receivedJsonMessage = JObject.Parse(JsonMessageGenerator.GetJsonStopSessionMessage(patientId));
                        }
                    }
                    dokterMessage = JObject.Parse(ReadJsonMessage(tcpClient));
                }
            }
        }

        public void SaveSession(List<JObject> sessionData)
        {
            WriteJsonMessage(tcpClient, JsonMessageGenerator.GetJsonOkMessage("server/received"));
        }

        public static void WriteJsonMessage(TcpClient client, string jsonMessage)
        {
            Console.WriteLine("Write: " + jsonMessage);
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            byte[] RequestLength = BitConverter.GetBytes(jMessage.Length);
            byte[] request = Encoding.ASCII.GetBytes(jMessage);
            {
                stream.Write(jsonMessage);
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

                Console.WriteLine("Read: " + message);

                return message;
            }
        }
    }
}