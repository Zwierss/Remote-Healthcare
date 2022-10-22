using System.Net.Sockets;
using DoctorLogic.commandhandlers.client;
using DoctorLogic.commandhandlers.server;
using Newtonsoft.Json.Linq;
using static DoctorLogic.Cryptographer;
using static DoctorLogic.PacketSender;
using static DoctorLogic.Util;

namespace DoctorLogic;

public class DoctorClient
{
    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];
    
    private TcpClient _tcp;
    private NetworkStream _stream;
    private readonly Dictionary<string, ICommand> _commands;

    private readonly string _hostname;
    private readonly int _port;

    private string _uuid;
    private string _password;

    public bool? LoginSuccessful { get; set; }

    public DoctorClient(string uuid, string password, string hostname, int port)
    {
        _uuid = uuid;
        _password = password;
        _hostname = hostname;
        _port = port;
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
    }

    public void Start()
    { 
        new Thread(Also).Start();
    }

    private async void Also()
    {
        await SetupConnection();
    }

    public async Task SetupConnection()
    {
        try
        {
            _tcp = new TcpClient();
            await _tcp.ConnectAsync(_hostname, _port);
            _stream = _tcp.GetStream();
        }
        catch (Exception)
        {
            Console.WriteLine("Couldn't connect to server");
        }
        
        SendData(SendReplacedObject("uuid", _uuid, 1, SendReplacedObject("pass", _password, 1, "server\\connect.json"))!);
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    private void OnRead(IAsyncResult ar)
    {
        try
        {
            int rc = _stream!.EndRead(ar);
            _totalBuffer = Concat(_totalBuffer, _buffer, rc);
        }
        catch(IOException)
        {
            Console.WriteLine("Can no longer read from this server");
            return;
        }

        while (_totalBuffer.Length >= 4)
        {
            JObject data = GetDecryptedMessage(_totalBuffer);
            Console.WriteLine(data);
            _totalBuffer = Array.Empty<byte>();

            if (_commands.ContainsKey(data["id"]!.ToObject<string>()!))
                _commands[data["id"]!.ToObject<string>()!].OnCommandReceived(data,this);

            break;
        }

        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    public void SendData(JObject message)
    {
        byte[] encryptedMessage = GetEncryptedMessage(message);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    private void InitCommands()
    {
        _commands.Add("client/server-connected", new ServerConnected());
        _commands.Add("doctor/return-clients", new ReturnClients());
        _commands.Add("doctor/return-client", new ReturnClient());
        _commands.Add("doctor/senddata", new ReceivedData());
    }
}