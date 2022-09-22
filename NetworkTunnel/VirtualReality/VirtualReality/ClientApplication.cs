using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VirtualReality
{
    internal class ClientApplication
    {
        public ClientApplication()
        {
            Connect();
        }

        public static void WriteTextMessage(TcpClient client, string message)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            {
                stream.WriteLine(message);
                stream.Flush();
            }
        }

        public static void Connect()
        {
            //145.49.20.104
            Console.WriteLine("Client started");
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("145.49.20.104"), 6666);
            bool done = false;
            Console.WriteLine("Type 'bye' to end connection");
            while (!done)
            {
                string response = ReadTextMessage(client);
                Console.Write("Enter a message to send to server: ");
                Console.WriteLine("Response: " + response);



                done = response.Equals("BYE");

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
