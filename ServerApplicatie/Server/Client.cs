#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DataSaving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Client
    {
        private static readonly byte[] Key =
        {
            0xF1, 0x22, 0xAA, 0x40, 0x7B, 0xBF, 0x32, 0x58,
            0x88, 0xE9, 0xAE, 0x69, 0x72, 0xF4, 0xBD, 0x5D
        };

        private static readonly byte[] IV =
        {
            0x41, 0xE4, 0x43, 0x42, 0x81, 0x5F, 0xDA, 0xE8,
            0x8E, 0x88, 0xA0, 0xFF, 0xEF, 0x6E, 0x5B, 0x54
        };

        private readonly TcpClient _tcpClient;
        private NetworkStream _stream = null!;

        public string PatientId { get; set; } = null!;

        private readonly List<JObject> _sessionData = new();
        
        public Client(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;

            string okMessage = JsonMessageGenerator.GetJsonOkMessage("client/connected");
            WriteJsonMessage(this._tcpClient, okMessage + "\r\n");
            Thread thread = new Thread(HandleClient);
            thread.Start();
        }

        public void HandleClient()
        {
            bool loggedIn = false;
            while (!loggedIn)
            {
                JObject loginRequest = JObject.Parse(ReadJsonMessage(_tcpClient));
                string patientId = loginRequest["data"]["patientId"].ToString();
                if(DataSaver.ClientExists(patientId))
                {
                    PatientId = patientId;
                    WriteTextMessage(_tcpClient, JsonMessageGenerator.GetJsonLoggedinMessage(false) + "\n");
                }
                else
                {
                    PatientId = patientId;
                    DataSaver.AddNewClient(this);
                    WriteTextMessage(_tcpClient, JsonMessageGenerator.GetJsonLoggedinMessage(true) + "\n");
                }
            }
            while (true)
            {
                string jsonSessionData = ReadJsonMessage(_tcpClient);
                Console.WriteLine(jsonSessionData);
                JObject jsonMessage = JObject.Parse(jsonSessionData);
                if ((bool) jsonMessage["data"]!["endOfSession"]!)
                {
                    _sessionData.Add(jsonMessage);
                    SaveSession(_sessionData);
                    _sessionData.RemoveRange(0, _sessionData.Count);
                    WriteJsonMessage(_tcpClient, JsonMessageGenerator.GetJsonOkMessage("client/received"));
                }
                else
                {
                    _sessionData.Add(jsonMessage);
                }
            }
        }

        public void SaveSession(List<JObject> sessionData)
        {
            WriteTextMessage(_tcpClient, JsonMessageGenerator.GetJsonOkMessage("client/received"));
            DataSaver.AddPatientFile(_tcpClient, sessionData);
        }

        public static void WriteJsonMessage(TcpClient client, string jsonMessage)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
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

        private static JObject ReadMessage(TcpClient client)
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