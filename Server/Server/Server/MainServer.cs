using System.Net;
using System.Net.Sockets;

namespace Server;

public class MainServer
{

    private TcpListener _listener;

    public List<Client> Clients { get; }
    public List<Client> OnlineClients { get; }

    private const string Hostname = "localhost";
    private const int Port = 6666;
    
    /* The constructor of the class. It is called when an instance of the class is created. */
    public MainServer()
    {
        Clients = new List<Client>();
        OnlineClients = new List<Client>();
        _listener = new TcpListener(IPAddress.Any, Port);
        _listener.Start();
        _listener.BeginAcceptTcpClient(OnConnect, null);
    }

    /// <summary>
    /// "When a new client connects, create a new Client object, add it to the list of clients, and start listening for new
    /// clients again."
    /// 
    /// The Client class is a class that I created to hold the information about each client. It has a TcpClient object, a
    /// NetworkStream object, and a byte array for receiving data
    /// </summary>
    /// <param name="IAsyncResult">This is an object that represents the status of an asynchronous operation.</param>
    private void OnConnect(IAsyncResult ar)
    {
        Console.WriteLine("new client connected");
        TcpClient tcp = _listener.EndAcceptTcpClient(ar);
        Client client = new(tcp,this);
        Clients.Add(client);
        _listener.BeginAcceptTcpClient(OnConnect, null);
    }
}