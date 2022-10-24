using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Server.DataSaving;

namespace Server
{
    class Program
    {
        //private static TcpListener listener;
        public static List<Client> clients = new List<Client>();


        static void Main(string[] args)
        {
            //DataSaver.GetPatientData();
            Network network = new Network();

            Thread thread = new Thread(network.RunServer);
            thread.Start();
        }

        /*
        private static void OnConnect(IAsyncResult ar)
        {
            var tcpClient = listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");
            Client newClient = new Client(tcpClient);
            newClient.patientId = "Kars";
            clients.Add(newClient);
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        internal void Disconnect(Client client)
        {
            clients.Remove(client);
            Console.WriteLine("Client disconnected");
        }
        */
    }
}