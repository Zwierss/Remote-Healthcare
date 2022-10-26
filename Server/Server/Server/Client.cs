using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using Server.commandhandlers;
using Server.commandhandlers.client;
using Server.commandhandlers.doctor;
using static Server.Cryptographer;

namespace Server;

public class Client
{

    private readonly TcpClient _tcp;
    private readonly NetworkStream _stream;
    private readonly Dictionary<string, ICommand> _commands;

    public MainServer Parent { get;}
    public bool SessionActive { get; set; }
    public string CurrentSessionTitle { get; set; }
    public bool IsDoctor { get; set; }
    public string Uuid { get; set; }
    public StorageManager StorageManager { get; }

    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    /* This is the constructor for the Client class. It initializes the client with a TcpClient, a MainServer, and a
    NetworkStream. It also initializes the commands dictionary and starts reading from the stream. */
    public Client(TcpClient tcp, MainServer parent)
    {
        StorageManager = new StorageManager();
        SessionActive = false;
        Parent = parent;
        _tcp = tcp;
        _stream = _tcp.GetStream();
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    /// <summary>
    /// It takes a JObject, encrypts it, and sends it to the server
    /// </summary>
    /// <param name="JObject">This is the JSON object that you want to send to the server.</param>
    public void SendMessage(JObject packet)
    {
        byte[] encryptedMessage = GetEncryptedMessage(packet);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    /// <summary>
    /// It encrypts the message and sends it to the server.
    /// </summary>
    /// <param name="JObject">The JSON object to be sent.</param>
    /// <returns>
    /// A Task object.
    /// </returns>
    private async Task<Task> SendMessageAsync(JObject packet)
    {
        byte[] encryptedMessage = GetEncryptedMessage(packet);
        await _stream.WriteAsync(encryptedMessage, 0, encryptedMessage.Length);
        return Task.CompletedTask;
    }

    /// <summary>
    /// It reads data from the client, decrypts it, and then passes it to the appropriate command handler
    /// </summary>
    /// <param name="IAsyncResult">This is the result of the asynchronous operation.</param>
    private void OnRead(IAsyncResult ar)
    {
        try
        {
            int rc = _stream.EndRead(ar);
            _totalBuffer = Concat(_totalBuffer, _buffer, rc);
            
            while (_totalBuffer.Length >= 4)
            {
                JObject data = GetDecryptedMessage(_totalBuffer);
                _totalBuffer = Array.Empty<byte>();

                if (_commands.ContainsKey(data["id"]!.ToObject<string>()!))
                    _commands[data["id"]!.ToObject<string>()!].OnCommandReceived(data,this);

                break;
            }

            
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch(Exception)
        {
            Console.WriteLine("Can no longer read from this client");
            SelfDestruct(false);
        }
    }

    /// <summary>
    /// It sends the client list to the client, removes the client from the server's client list, removes the client from
    /// the server's online client list, and then closes the client's connection
    /// </summary>
    /// <param name="disconnect">If true, the client will be disconnected from the server.</param>
    /// <returns>
    /// A Task object.
    /// </returns>
    public async void SelfDestruct(bool disconnect)
    {
        await SendClientList();
        Parent.Clients.Remove(this);
        Parent.OnlineClients.Remove(this);
        if (!disconnect) return;
        _stream.Close(2000);
        _tcp.Close();
    }

    /// <summary>
    /// It sends a list of all the clients in the server to all the doctors
    /// </summary>
    /// <returns>
    /// A list of clients
    /// </returns>
    private async Task<Task> SendClientList()
    {
        if (IsDoctor) return Task.CompletedTask;
        List<string> clientUuids = new();
        List<Client> doctors = new();
        foreach (Client c in Parent.Clients)
        {
            if (!c.IsDoctor)
            {
                clientUuids.Add(c.Uuid);
            }
            else
            {
                doctors.Add(c);
            }
        }

        foreach (Client c in doctors)
        {
            await c.SendMessageAsync(PacketSender.SendReplacedObject("clients", clientUuids, 1, "doctor\\returnclients.json")!);    
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// It adds all the commands to the dictionary
    /// </summary>
    private void InitCommands()
    {
        _commands.Add("server/client-enter", new NewClient());
        _commands.Add("doctor/senddata", new SendData());
        _commands.Add("server/getclients", new GetClient());
        _commands.Add("client/startsession", new StartSession());
        _commands.Add("client/stopsession", new StopSession());
        _commands.Add("client/emergencystop", new Switch());
        _commands.Add("client/doctormessage", new Switch());
        _commands.Add("client/setresistance", new Switch());
        _commands.Add("server/disconnect", new Disconnect());
        _commands.Add("server/create-account", new CreateAccount());
        _commands.Add("server/ready", new ClientReady());
        _commands.Add("doctor/subscribed", new Switch());
        _commands.Add("client/subscribe", new Switch());
        _commands.Add("client/unsubscribe", new Switch());
        _commands.Add("server/get-offline", new GetOffline());
        _commands.Add("server/getsessions", new GetSessions());
        _commands.Add("server/getsessiondata", new GetSessionData());
    }
    
    /// <summary>
    /// It takes two byte arrays and a count, and returns a new byte array that is the concatenation of the first two
    /// arrays, with the second array truncated to the specified count
    /// </summary>
    /// <param name="b1">The first byte array to concatenate.</param>
    /// <param name="b2">The byte array to be appended to b1</param>
    /// <param name="count">The number of bytes to copy from the second array.</param>
    /// <returns>
    /// The concatenated byte array.
    /// </returns>
    private static byte[] Concat(byte[] b1, byte[] b2, int count)
    {
        byte[] r = new byte[b1.Length + count];
        Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
        Buffer.BlockCopy(b2, 0, r, b1.Length, count);
        return r;
    }
}