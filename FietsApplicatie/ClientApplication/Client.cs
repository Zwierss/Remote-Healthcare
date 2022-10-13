using System.Net.Sockets;
using System.Text;
using ClientApplication.commandhandlers.doctor;
using ClientApplication.commandhandlers.server;
using FietsDemo;
using Newtonsoft.Json.Linq;
using VirtualReality;
using static VirtualReality.PacketSender;
using static ClientApplication.Cryptograhper;
using static VirtualReality.VRClient;
using static FietsDemo.HardwareConnector;

namespace ClientApplication;

public class Client : IClientCallback
{
    private static readonly string Path = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("ClientApplication", StringComparison.Ordinal)) + "ClientApplication\\packets\\";
    
    private readonly VRClient _vr;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private readonly Dictionary<string, ICommand> _commands;

    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public string Username { get; set; }
    public bool SessionIsActive { get; set; }
    public bool ConnectedToServer { get; set; }

    private readonly string _bikeSerial;
    private readonly string _hostname;
    private readonly int _port;

    public Client(string username, string bikeSerial, string hostname, int port)
    {
        _vr = new VRClient();
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        Username = username;
        _bikeSerial = bikeSerial;
        _hostname = hostname;
        _port = port;
        SessionIsActive = false;
        ConnectedToServer = false;
    }

    public async Task SetupConnection()
    {
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(_hostname, _port);
            //await _vr.StartConnection();
            _stream = _client.GetStream();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        // while (!Connected)
        // {
        //     await SetupHardware(this, _bikeSerial);
        // }
        
        SendData(SendReplacedObject<string, string>("uuid", Username, 1, "server\\init.json")!);

        _stream!.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    public void OnNewBikeData(IReadOnlyList<int> values)
    {
        double speed = ((values[8] + values[9] * 255) * 0.001) * 3.6;
        _vr.UpdateBikeSpeed(speed);
        if (!SessionIsActive) return;
    }

    public void OnNewHeartrateData(IReadOnlyList<int> values)
    {
        _vr.UpdatePanel(values[1]);
        if (!SessionIsActive) return;
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

    private void SendData(JObject message)
    {
        byte[] encryptedMessage = GetEncryptedMessage(message);
        _stream!.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    private void InitCommands()
    {
        _commands.Add("server/connected", new ServerConnected());
        _commands.Add("server/login", new ServerLogin());
        _commands.Add("server/received", new ServerReceived());
        _commands.Add("doctor/emergencystop", new EmergencyStop());
        _commands.Add("doctor/startsession", new StartSession());
        _commands.Add("doctor/endsession", new EndSession());
        _commands.Add("doctor/send", new SendDoctor());
    }
}