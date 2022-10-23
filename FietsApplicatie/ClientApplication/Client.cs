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
using static ClientApplication.State;

namespace ClientApplication;

public class Client : IClientCallback
{
    private readonly VRClient _vr;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private readonly Dictionary<string, ICommand> _commands;

    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public string Username { get; set; }
    public bool SessionIsActive { get; set; }
    public bool ConnectedToServer { get; set; }
    
    public ClientCallback Callback { get; set; }

    private int _lastHeartrateData = 0;

    public Client()
    {
        _vr = new VRClient();
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        SessionIsActive = false;
        ConnectedToServer = false;
    }

    public async void SetupConnection(string username, string password, string hostname, int port)
    {
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(hostname, port);
            _stream = _client.GetStream();
        }
        catch(Exception e)
        {
            Callback.OnCallback(Error, "Kon geen verbinding maken met deze server.");
            return;
        }
        
        SendData(SendReplacedObject<string, JObject>("uuid", username, 1, SendReplacedObject<string,string>(
            "pass", password, 1, "application\\server\\init.json"
        )!)!);
        _stream!.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    public async void SetupRest()
    {
        await _vr.StartConnection();
        
        while (!_vr.IsSet)
        {
            
        }

        SetupHardware(this, "01140");
    }

    public void SelfDestruct()
    {
        _stream?.Close(400);
        _client?.Close();
    }

    public void OnNewBikeData(IReadOnlyList<int> values)
    {
        Console.WriteLine(ConnectedToServer);

        if(!ConnectedToServer) return;
        if (!_vr.IsSet) return;
        
        double speed = ((values[8] + values[9] * 255) * 0.001) * 3.6;
        _vr.UpdateBikeSpeed(speed);
        
        if (!SessionIsActive) return;

        JObject o = SendReplacedObject("client-id", Username, 1, SendReplacedObject(
            "speed", speed, 2, SendReplacedObject(
                "heartrate", _lastHeartrateData, 2, SendReplacedObject(
                    "distance", values[7], 2, SendReplacedObject(
                        "time", Time, 2, SendReplacedObject(
                            "receiver", "10", 2, "application\\doctor\\senddata.json"
                        )
                    )
                )
            )
        ))!;
        
        SendData(o);
    }

    public void OnNewHeartrateData(IReadOnlyList<int> values)
    {
        if(!ConnectedToServer) return;
        if (!_vr.IsSet) return;
        
        _vr.UpdatePanel(values[1]);
        
        if (!SessionIsActive) return;

        _lastHeartrateData = values[1];
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

        try
        {
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch (Exception)
        {
            Console.WriteLine("Stream closed");
        }
    }

    private void SendData(JObject message)
    {
        byte[] encryptedMessage = GetEncryptedMessage(message);
        _stream!.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    private void InitCommands()
    {
        _commands.Add("client/server-connected", new ServerConnected());
        _commands.Add("client/startsession", new StartSession());
        _commands.Add("client/stopsession", new StopSession());
        _commands.Add("client/emergencystop", new EmergencyStop());
        _commands.Add("client/doctormessage", new DoctorMessage());
        _commands.Add("client/changeresistance", new ChangeResistance());
    }
}