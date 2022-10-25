#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DataSaving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Client
    {
        private static readonly byte[] Key =
        {
            0xF1, 0x22, 0xAA, 0x40, 0x7B, 0xBF, 0x32, 0x58,
            0x88, 0xE9, 0xAE, 0x69, 0x72, 0xF4, 0xBD, 0x5D
        };
        public TcpClient tcpClient;
        public NetworkStream stream;
        public byte[] buffer = new byte[1024];
        public string totalBuffer = "";

        private static readonly byte[] IV =
        {
            0x41, 0xE4, 0x43, 0x42, 0x81, 0x5F, 0xDA, 0xE8,
            0x8E, 0x88, 0xA0, 0xFF, 0xEF, 0x6E, 0x5B, 0x54
        };

        private readonly TcpClient _tcpClient;
        private NetworkStream _stream = null!;

        public JObject receivedJsonMessage = new JObject();

        public JObject receivedJsonData = new JObject();

        public string PatientId;

        public class Patient
        {
            public string patientId { get; set; }
        }

        private List<JObject> _sessionData = new();
        
        public Client(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;

            string okMessage = JsonMessageGenerator.GetJsonOkMessage("client/connected");
            WriteJsonMessage(this._tcpClient, okMessage + "\r\n");
            Thread thread = new Thread(HandleConnection);
            thread.Start();
        }
        
        public Client()
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
                        this.PatientId = "Dokter";
                        string loginConfirm = JsonMessageGenerator.GetJsonOkMessage("server/login");
                        
                        WriteJsonMessage(tcpClient, loginConfirm + "\n");

                        HandleDoctor();

                    }
                }
                else if (loginRequest["id"].ToString() == "client/login")
                {
                    if (DataSaver.ClientExists(loginRequest["data"]["patientId"].ToString()))
                    {
                        this.PatientId = loginRequest["data"]["patientId"].ToString();
                        WriteJsonMessage(tcpClient, JsonMessageGenerator.GetJsonLoggedinMessage(false) + "\n");
                    }
                    else
                    {
                        this.PatientId = loginRequest["data"]["patientId"].ToString();
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
                            if (Program.clients[i].PatientId == "Dokter")
                            {
                                Program.clients[i]._sessionData = patientSessionData;
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
                        if(patientId == Program.clients[i].PatientId)
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
                            Trace.WriteLine("if statement: " + this._sessionData.Count);
                            while(this._sessionData.Count == 0)
                            {

                            }
                            if (this._sessionData.Count > 0)
                            {
                                Trace.WriteLine("Session data: " + this._sessionData[this._sessionData.Count - 1]);
                                WriteJsonMessage(tcpClient, this._sessionData[this._sessionData.Count - 1].ToString() + "\n");
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
                        if (patientId == Program.clients[i].PatientId)
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
            byte[] RequestLength = BitConverter.GetBytes(jsonMessage.Length);
            byte[] request = Encoding.ASCII.GetBytes(jsonMessage);
            {
                stream.Write(jsonMessage);
                stream.Flush();
            }
        }

        private static void WriteMessage(TcpClient client, JObject packet)
        {
            StreamWriter sw = new(client.GetStream(), Encoding.ASCII);
            sw.Write(EncryptMessage(packet));
            sw.Flush();
        }

        public static JObject ReadMessage(TcpClient client)
        {
            StreamReader sr = new(client.GetStream(), Encoding.ASCII);
            byte[] incomingMessage = Array.Empty<byte>();
            int count = 0;
            while (sr.Peek() != -1)
            {
                incomingMessage[count] = (byte)sr.Read();
                count++;
            }

            return DecryptMessage(incomingMessage);
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

                Console.WriteLine("Read: " + message);

                return message;
            }
        }

        public static string? ReadTextMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                return stream.ReadLine();
            }
        }

        public static byte[] EncryptMessage(JObject o)
        {
            string message = o.ToString();
            byte[] data = Encoding.ASCII.GetBytes(message);
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using ICryptoTransform ct = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, ct, CryptoStreamMode.Write);
            cs.Write(data,0,data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        public static JObject DecryptMessage(byte[] message)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using ICryptoTransform ct = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, ct, CryptoStreamMode.Write);
            cs.Write(message,0,message.Length);
            cs.FlushFinalBlock();
            return JObject.Parse(Encoding.Default.GetString(ms.ToArray()));
        }
    }
}