using Server.DataSaving;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Client
    {
        #region connection stuff
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private string totalBuffer = "";
        #endregion

        public string patientId { get; set; }


        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;

            //ClientLogin();

            //Thread thread = new Thread(HandleData);
            //thread.Start();

            //this.userName = userName;

            //this.stream = this.tcpClient.GetStream();
            //stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        
        public Client()
        {
            this.tcpClient = null;

            //ClientLogin();

            //Thread thread = new Thread(HandleData);
            //thread.Start();

            //this.userName = userName;

            //this.stream = this.tcpClient.GetStream();
            //stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        public void ClientLogin()
        {
            //Voer patient id in: \n
            WriteTextMessage(this.tcpClient, "Welkom!\n");
            string patientId = ReadTextMessage(tcpClient);
            Console.WriteLine(patientId);

            if (DataSaver.ClientExists(patientId))
            {
                WriteTextMessage(this.tcpClient, "Patient gevonden\n");
            }
            else
            {
                this.patientId = patientId;
                DataSaver.AddNewClient(this);
                WriteTextMessage(this.tcpClient, "Nieuw account aangemaakt\n");
            }
        }

        public void HandleData()
        {
            bool connected = true;
            while (connected)
            {
                string received = ReadTextMessage(this.tcpClient);
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

        public static string ReadTextMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                return stream.ReadLine();
            }
        }

        /*
        private bool assertPacketData(string[] packetData, int requiredLength)
        {
            if (packetData.Length < requiredLength)
            {
                Write("error");
                return false;
            }
            return true;
        }
        */

        /*
        public void Write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data + "\r\n\r\n");
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }
        */
    }
}