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
    private bool _isActive;

    public string Uuid { get; set; }

    /* Initializing the commands dictionary and setting the _isActive variable to false. */
    public DoctorClient()
    {
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        _isActive = false;
    }

    /// <summary>
    /// It connects to the server, sends the UUID and password, and starts listening for incoming data
    /// </summary>
    /// <param name="uuid">The uuid of the user.</param>
    /// <param name="password">The password of the server.</param>
    /// <param name="hostname">The hostname of the server.</param>
    /// <param name="port">The port to connect to.</param>
    /// <returns>
    /// The return value is a string.
    /// </returns>
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
    
    /// <summary>
    /// It connects to the server, sends a json object with the username and password, and then waits for a response
    /// </summary>
    /// <param name="username">The username of the account you want to create</param>
    /// <param name="password">The password of the account</param>
    /// <param name="hostname">The hostname of the server</param>
    /// <param name="port">The port of the server</param>
    /// <returns>
    /// A string
    /// </returns>
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
    
    /// <summary>
    /// > Connects to a server and sets up the stream for reading and writing
    /// </summary>
    /// <param name="hostname">The hostname of the server you want to connect to.</param>
    /// <param name="port">The port to connect to.</param>
    private async Task Connect(string hostname, int port)
    {
        _tcp = new TcpClient();
        await _tcp.ConnectAsync(hostname, port);
        _stream = _tcp.GetStream();
        _isActive = true;
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Net.Sockets.OverlappedAsyncResult")]
    /// <summary>
    /// It reads the data from the stream, decrypts it, and then calls the appropriate command handler
    /// </summary>
    /// <param name="IAsyncResult">The result of the asynchronous operation.</param>
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
                Console.WriteLine(data);
            
                if (_commands.ContainsKey(data["id"]!.ToObject<string>()!))
                    _commands[data["id"]!.ToObject<string>()!].OnCommandReceived(data,this);

                break;
            }
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch (Exception e)
        {
            Stop(false);
        }
    }

    /// <summary>
    /// It closes the stream and the TCP connection
    /// </summary>
    public void SelfDestruct()
    {
        _stream.Close(400);
        _tcp.Close();
    }

    /// <summary>
    /// It sends a message to the server to disconnect the client
    /// </summary>
    /// <param name="notify">Whether or not to notify the server that the client is disconnecting.</param>
    public void Stop(bool notify)
    {
        SendData(SendReplacedObject("client", Uuid, 1, SendReplacedObject("notify", notify, 1, "server\\disconnect.json"))!);
    }

    /// <summary>
    /// This function sends a JSON object to the server to stop the robot
    /// </summary>
    /// <param name="client">The client's name</param>
    public void EmergencyStop(string client)
    {
        SendData(SendReplacedObject("client", client, 1, "client\\emergencystop.json")!);
    }

    /// <summary>
    /// It sends a request to the server to start a session with the client
    /// </summary>
    /// <param name="client">The client's UUID</param>
    public void StartSession(string client)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "doctor", Uuid, 1, "client\\startsession.json"
        ))!);
    }

    /// <summary>
    /// It stops the session of the client.
    /// </summary>
    /// <param name="client">The client's name</param>
    public void StopSession(string client)
    {
        SendData(SendReplacedObject("client", client, 1, "client\\stopsession.json")!);
    }

    /// <summary>
    /// It sends a message to a client from a doctor
    /// </summary>
    /// <param name="client">The client to send the message to</param>
    /// <param name="messge">The message to send</param>
    public void SendDoctorMessage(string client, string messge)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "sender", Uuid, 2, SendReplacedObject(
                "message", messge, 2, "client\\doctormessage.json"
            )
        ))!);
    }

    /// <summary>
    /// It sends a packet to the server to change the resistance of a client
    /// </summary>
    /// <param name="client">The client's name</param>
    /// <param name="value">The resistance value to change to.</param>
    public void ChangeResistance(string client, int value)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "resistance", value, 2, "client\\changeresistance.json"
        ))!);
    }

    /// <summary>
    /// It takes a JObject, encrypts it, and sends it to the server
    /// </summary>
    /// <param name="JObject">This is the message that you want to send.</param>
    /// <returns>
    /// The encrypted message
    /// </returns>
    private void SendData(JObject message)
    {
        if (!_isActive) return;
        byte[] encryptedMessage = GetEncryptedMessage(message);
        try
        {
            _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
        }
        catch (Exception)
        {
            Console.WriteLine("problem while writing");
        }
    }

    /// <summary>
    /// It sends a JSON file to the server
    /// </summary>
    public void GetClients()
    {
        SendData(GetJson("server\\getclients.json"));
    }

    /// <summary>
    /// > This function sends a request to the server to get a list of all the offline clients
    /// </summary>
    public void GetOfflineClients()
    {
        SendData(GetJson("server\\getoffline.json"));
    }

    /// <summary>
    /// "Send the server a request to get all the sessions for a client."
    /// 
    /// The first parameter is the client name. The second parameter is the number of the request. The third parameter is
    /// the path to the JSON file that contains the request. The fourth parameter is the request type
    /// </summary>
    /// <param name="client">The client's name</param>
    public void GetSessions(string client)
    {
        SendData(SendReplacedObject("client", client, 1, "server\\getsessions.json")!);
    }

    /// <summary>
    /// It sends a request to the server to get the session data of a client
    /// </summary>
    /// <param name="client">The client's name</param>
    /// <param name="item">The item to get the data from.</param>
    public void GetSessionData(string client, string item)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "file", item, 1, "server\\getsessiondata.json"
        ))!);
    }

    /// <summary>
    /// It sends a request to the server to subscribe to a doctor.
    /// </summary>
    /// <param name="doctor">The doctor's username</param>
    /// <param name="client">The client's name</param>
    public void Subscribe(string doctor, string client)
    {
        SendData(SendReplacedObject("client", client, 1, SendReplacedObject(
            "doctor", doctor, 1, "client\\subscribe.json"
        ))!);
    }

    /// <summary>
    /// It sends a request to the server to unsubscribe a client from a channel.
    /// </summary>
    /// <param name="client">The client's name</param>
    public void Unsubscribe(string client)
    {
        SendData(SendReplacedObject("client", client, 1, "client\\unsubscribe.json")!);
    }

    /// <summary>
    /// It adds all the commands to the dictionary
    /// </summary>
    private void InitCommands()
    {
        _commands.Add("client/server-connected", new ServerConnected());
        _commands.Add("client/account-created", new AccountCreated());
        _commands.Add("doctor/return-clients", new ReturnClients());
        _commands.Add("doctor/return-client", new ReturnClient());
        _commands.Add("doctor/senddata", new ReceivedData());
        _commands.Add("doctor/subscribed", new Subscribed());
        _commands.Add("doctor/return-offline", new ReturnOffline());
        _commands.Add("doctor/return-sessions", new ReturnSessions());
        _commands.Add("doctor/return-sessiondata", new ReturnSessionData());
        _commands.Add("doctor/loading-complete", new LoadingComplete());
    }
}