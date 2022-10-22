using System.Net;
using System.Net.Sockets;

namespace Server;

public class MainServer
{

    private TcpListener _listener;

    public List<Client> Clients { get; set; }

    private const string Hostname = "localhost";
    private const int Port = 6666;
    
    public MainServer()
    {
        Clients = new List<Client>();
        _listener = new TcpListener(IPAddress.Any, Port);
        _listener.Start();
        _listener.BeginAcceptTcpClient(OnConnect, null);
    }

    private void OnConnect(IAsyncResult ar)
    {
        TcpClient tcp = _listener.EndAcceptTcpClient(ar);
        Client client = new(tcp,this);
        Clients.Add(client);
        _listener.BeginAcceptTcpClient(OnConnect, null);
    }
}