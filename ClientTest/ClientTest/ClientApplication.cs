using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace ClientTest
{
    internal class ClientApplication
    {
        private static string password;
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static string username;

        public ClientApplication()
        {
            Console.WriteLine(GetLocalIPAddress());
            client = new TcpClient();
            client.BeginConnect("192.168.43.137", 15243, new AsyncCallback(OnConnect), null);

            Console.ReadLine();
            Console.ReadLine();
        }

        private static void OnConnect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            Console.WriteLine("Verbonden!");
            //stream = client.GetStream();
            handlingServer();
        }

        public static void handlingServer()
        {
            while (true)
            {
                Console.WriteLine(ReadTextMessage(client));
                //string? messageLine = Console.ReadLine();
                WriteTextMessage(client, "1234\n");
                Console.WriteLine(ReadTextMessage(client));

            }
        }

        public static string? ReadTextMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                return stream.ReadLine();
            }
        }

        public static void WriteTextMessage(TcpClient client, string? message)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            {
                stream.Write(message);
                stream.Flush();
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}

