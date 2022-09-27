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

namespace VirtualReality
{
    internal class ClientApplication
    {
        private static string password;
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static string username;

        private static bool loggedIn = false;
        public ClientApplication()
        {
            Console.WriteLine("Client started");
            client = new TcpClient();
            client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);

           

            //boolean firstconnectmessage = true;

            //while (true)
            //{
            //    write($"chat\r\n{""}");
            //}

            //while (true)
            //{
            //    console.writeline("voer een chatbericht in:");
            //    string newchatmessage = console.readline();
            //    if (loggedin)
            //        write($"chat\r\n{newchatmessage}");
            //    else
            //        console.writeline("je bent nog niet ingelogd");
            //}
        }

        private static void OnConnect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            Console.WriteLine("Verbonden!");

            Console.WriteLine(ReadTextMessage(client));
            Console.WriteLine("Voer cliëntnummer in");
            username = Console.ReadLine();
            WriteTextMessage(client, username);
            Console.WriteLine(ReadTextMessage(client));

            stream = client.GetStream();

            // stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            Console.WriteLine(ReadTextMessage(client));
            // write($"login\r\n{username}\r\n{password}");


        }

        public static string ReadTextMessage(TcpClient client)
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
                stream.Write(message); stream.Flush();
            }
        }

        // private static void OnRead(IAsyncResult ar)
        // {
        //     int receivedBytes = stream.EndRead(ar);
        //     string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
        //     totalBuffer += receivedText;
        //
        //     while (totalBuffer.Contains("\r\n\r\n"))
        //     {
        //         string packet = totalBuffer.Substring(0, totalBuffer.IndexOf("\r\n\r\n"));
        //         totalBuffer = totalBuffer.Substring(totalBuffer.IndexOf("\r\n\r\n") + 4);
        //         string[] packetData = Regex.Split(packet, "\r\n");
        //         handleData(packetData);
        //     }
        //     stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        // }

        private static void write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }

        // private static void handleData(string[] packetData)
        // {
        //     Console.WriteLine($"Packet ontvangen: {packetData[0]}");
        //
        //     switch (packetData[0])
        //     {
        //         case "login":
        //             if (packetData[1] == "ok")
        //             {
        //                 Console.WriteLine("Logged in!");
        //                 loggedIn = true;
        //             }
        //             else
        //                 Console.WriteLine(packetData[1]);
        //             break;
        //         case "chat":
        //             Console.WriteLine($"Chat ontvangen: '{packetData[1]}'");
        //             break;
        //     }
        //
        // }
    }
}


