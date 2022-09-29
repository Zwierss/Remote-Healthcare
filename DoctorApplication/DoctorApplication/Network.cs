using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DoctorApplication
{
    class Network
    {
        private static TcpClient tcpClient;

        private static string username = "";
        private static string password = "";

        public void StartConnection()
        {
            tcpClient = new TcpClient();
            tcpClient.BeginConnect("192.168.43.137", 15243, new AsyncCallback(OnConnect), null);

        }

        private static void OnConnect(IAsyncResult ar)
        {
            tcpClient.EndConnect(ar);
            Console.WriteLine("Verbonden!");
            HandlingServer();
        }

        public static void HandlingServer()
        {
            while (true)
            {
                JObject connectedMessage = JObject.Parse(ReadJsonMessage(tcpClient));
                if (connectedMessage["status"].ToString() == "ok")
                {
                    WriteTextMessage(tcpClient, JsonMessageGenerator.GetJsonLoginMessage(username, password));
                }
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
