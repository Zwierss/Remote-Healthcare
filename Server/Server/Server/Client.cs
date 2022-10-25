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

    public void SendMessage(JObject packet)
    {
        byte[] encryptedMessage = GetEncryptedMessage(packet);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    private async Task<Task> SendMessageAsync(JObject packet)
    {
        byte[] encryptedMessage = GetEncryptedMessage(packet);
        await _stream.WriteAsync(encryptedMessage, 0, encryptedMessage.Length);
        return Task.CompletedTask;
    }

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
            SelfDestruct();
        }
    }

    public async void SelfDestruct()
    {
        await SendClientList();
        Parent.Clients.Remove(this);
        Parent.OnlineClients.Remove(this);
        _stream.Close(400);
        _tcp.Close();
    }

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
    
    private static byte[] Concat(byte[] b1, byte[] b2, int count)
    {
        byte[] r = new byte[b1.Length + count];
        Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
        Buffer.BlockCopy(b2, 0, r, b1.Length, count);
        return r;
    }
}