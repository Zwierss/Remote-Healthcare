using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    public class Network
    {
        private TcpListener listener;
        public List<Client> clients = new List<Client>();

        public void RunServer()
        {
            Console.WriteLine("Hello Server!");

            listener = new TcpListener(IPAddress.Any, 15243);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);

            Console.ReadLine();
        }

        public void OnConnect(IAsyncResult ar)
        {
            var tcpClient = listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");
            Client newClient = new Client(tcpClient);
            //newClient.patientId = "Kars";
            clients.Add(newClient);
            Program.clients.Add(newClient);
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        public void Disconnect(Client client)
        {
            clients.Remove(client);
            Console.WriteLine("Client disconnected");
        }
    }
}
