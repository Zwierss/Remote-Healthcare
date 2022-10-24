using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using DoctorLogic.commandhandlers.client;
using DoctorLogic.commandhandlers.server;
using MvvmHelpers;
using Newtonsoft.Json.Linq;
using static DoctorLogic.Cryptographer;
using static DoctorLogic.PacketSender;
using static DoctorLogic.Util;
using static DoctorLogic.State;

namespace DoctorLogic;

public class DoctorClient
{
    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public IWindow ViewModel { get; set; }

    private TcpClient _tcp;
    private NetworkStream _stream;
    private readonly Dictionary<string, ICommand> _commands;

    public string Uuid { get; set; }

    public DoctorClient()
    {
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
    }

    public async void SetupConnection(string uuid, string password, string hostname, int port)
    {
        Uuid = uuid;
        
        try
        {
            await Connect(hostname, port);
        }
        catch (Exception)
        {
            string[] arg = { "Kon geen verbniding maken met de server. Mogelijk bent u niet verbonden via het juiste IP adres of poort."};
            ViewModel.OnChangedValues(Error, arg);
            return;
        }
        
        SendData(SendReplacedObject("uuid", Uuid, 1, SendReplacedObject("pass", password, 1, "server\\connect.json"))!);
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }
    
    public async void CreateAccount(string username, string password, string hostname, int port)
    {
        try
        {
            await Connect(hostname, port);
        }
        catch (Exception)
        {
            string[] arg = { "Kan niet verbinden met deze server" };
            ViewModel.OnChangedValues(Error, arg);
            return;
        }
        
        SendData(SendReplacedObject("uuid", username, 1, SendReplacedObject("pass", password, 1, "server\\createaccount.json"))!);
        _stream!.BeginRead(_buffer, 0, 1024, OnRead, null);
    }
    
    private async Task Connect(string hostname, int port)
    {
        _tcp = new TcpClient();
        await _tcp.ConnectAsync(hostname, port);
        _stream = _tcp.GetStream();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Net.Sockets.OverlappedAsyncResult")]
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
        catch (Exception)
        {
            Console.WriteLine("Stream closed");
            SelfDestruct();
        }
    }

    public void SelfDestruct()
    {
        _stream.Close(400);
        _tcp.Close();
    }

    public void Stop()
    {
        SendData(SendReplacedObject("client", Uuid, 1, "server\\disconnect.json")!);
    }

    public void EmergencyStop(string client)
    {
        SendData(SendReplacedObject("client", client, 1, "client\\emergencystop.json")!);
    }

    public void StartSession(string client)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "doctor", Uuid, 1, "client\\startsession.json"
        ))!);
    }

    public void StopSession(string client)
    {
        SendData(SendReplacedObject("client", client, 1, "client\\stopsession.json")!);
    }

    public void SendDoctorMessage(string client, string messge)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "sender", Uuid, 2, SendReplacedObject(
                "message", messge, 2, "client\\doctormessage.json"
            )
        ))!);
    }

    public void ChangeResistance(string client, int value)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "resistance", value, 2, "client\\changeresistance.json"
        ))!);
    }

    private void SendData(JObject message)
    {
        byte[] encryptedMessage = GetEncryptedMessage(message);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    public void GetClients()
    {
        SendData(GetJson("server\\getclients.json"));
    }

    public void Subscribe(string doctor, string client)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "doctor", doctor, 1, "client\\subscribe.json"
        ))!);
    }

    public void Unsubscribe(string client)
    {
        SendData(SendReplacedObject("client", client, 1, "client\\unsubscribe.json")!);
    }

    private void InitCommands()
    {
        _commands.Add("client/server-connected", new ServerConnected());
        _commands.Add("client/disconnected", new Disconnected());
        _commands.Add("client/account-created", new AccountCreated());
        _commands.Add("doctor/return-clients", new ReturnClients());
        _commands.Add("doctor/return-client", new ReturnClient());
        _commands.Add("doctor/senddata", new ReceivedData());
        _commands.Add("doctor/subscribed", new Subscribed());
    }
}