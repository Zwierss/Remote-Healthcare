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
            _tcp = new TcpClient();
            await _tcp.ConnectAsync(hostname, port);
            _stream = _tcp.GetStream();
        }
        catch (Exception)
        {
            ViewModel.OnChangedValues(Error, "Kon geen verbniding maken met de server. Mogelijk bent u niet verbonden via het juiste IP adres of poort.");
            return;
        }
        
        SendData(SendReplacedObject("uuid", Uuid, 1, SendReplacedObject("pass", password, 1, "server\\connect.json"))!);
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Net.Sockets.OverlappedAsyncResult")]
    private void OnRead(IAsyncResult ar)
    {
        try
        {
            int rc = _stream!.EndRead(ar);
            _totalBuffer = Concat(_totalBuffer, _buffer, rc);
        }
        catch(IOException)
        {
            return;
        }

        while (_totalBuffer.Length >= 4)
        {
            JObject data = GetDecryptedMessage(_totalBuffer);
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

    public void SelfDestruct()
    {
        _stream.Close(400);
        _tcp.Close();
    }

    public void SendData(JObject message)
    {
        byte[] encryptedMessage = GetEncryptedMessage(message);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    public void GetClients()
    {
        SendData(PacketSender.GetJson("server\\getclients.json"));
    }

    private void InitCommands()
    {
        _commands.Add("client/server-connected", new ServerConnected());
        _commands.Add("doctor/return-clients", new ReturnClients());
        _commands.Add("doctor/return-client", new ReturnClient());
        _commands.Add("doctor/senddata", new ReceivedData());
    }
}