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

namespace ClientApplication;

public class Client : IClientCallback
{
    private readonly VRClient _vr;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private Dictionary<string, ICommand> _commands;

    public string Username { get; set; }

    private const string Hostname = "";
    private const int Port = 0;

    public event EventHandler<byte[]>? OnMessage; 
    public Client()
    {
        _vr = new VRClient();
        OnMessage += (_ ,data) => HandleMessage(GetDecryptedMessage(data));
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        Username = "";
    }

    public async Task SetupConnection()
    {
        HardwareConnector.SetupHardware(this);
        await _vr.StartConnection();

        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(Hostname, Port);
            _stream = _client.GetStream();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }

    public void OnNewBikeData(IReadOnlyList<int> values)
    {
        double speed = ((values[8] + values[9] * 255) * 0.001) * 3.6;
        _vr.UpdateBikeSpeed(speed);
    }

    public void OnNewHeartrateData(IReadOnlyList<int> values)
    {
        _vr.UpdatePanel(values[1]);
    }

    private void HandleMessage(JObject packet)
    {
        try
        {
            _commands[packet["id"]!.ToObject<string>()!].OnCommandReceived(packet,this);
        }
        catch (Exception e)
        {
            Console.WriteLine("No such thing found as packet.id\n" + e.Message);
        }
    }

    private Task SendData(JObject message)
    {
        //byte[] messageLength = BitConverter.GetBytes(message.ToString().Length);
        byte[] encryptedMessage = GetEncryptedMessage(message);

        //_stream.Write(messageLength, 0, messageLength.Length);
        _stream!.Write(encryptedMessage, 0, encryptedMessage.Length);
        
        return Task.CompletedTask;
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