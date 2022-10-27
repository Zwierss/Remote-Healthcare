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

public class Client : IHardwareCallback
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
    public IClientCallback Callback { get; set; }
    public string Doctor { get; set; }
    public bool IsSubscribed { get; set; }
    public List<double> CollectedSpeeds { get; }
    public List<int> CollectedRates { get; }

    private int _prevDistance;
    /* A property that returns the value of _prevDistance. When the value is set, it checks if the value is 0. If it is, it
    sets _distanceCorrection to 0. */
    public int PrevDistance
    {
        get => _prevDistance;
        set
        {
            if (value == 0)
            {
                _distanceCorrection = 0;
            }
            _prevDistance = value;
        }
        
    }

    private bool _isActive;
    private int _lastHeartrateData;
    private string _bikeNr;
    private bool _sim;
    private int _distanceCorrection;
    private int _distanceLoopCounter;
    private string _vrs;

    /* This is the constructor of the Client class. It initializes the VRClient, the commands dictionary, the
    sessionIsActive, connectedToServer, isActive, distanceCorrection, distanceLoopCounter, prevDistance,
    collectedSpeeds, collectedRates and isSubscribed variables. */
    public Client()
    {
        _vr = new VRClient();
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        SessionIsActive = false;
        ConnectedToServer = false;
        _isActive = false;
        _distanceCorrection = 0;
        _distanceLoopCounter = 0;
        _prevDistance = 0;
        CollectedSpeeds = new List<double>();
        CollectedRates = new List<int>();
        IsSubscribed = false;
    }

    /// <summary>
    /// It connects to the server, sends the username and password, and starts listening for incoming data
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="hostname">The hostname of the server.</param>
    /// <param name="port">The port to connect to.</param>
    /// <param name="bikeNr">The number of the bike you want to connect to.</param>
    /// <param name="sim">Whether or not the client is a simulator.</param>
    /// <returns>
    /// The return type is void, so nothing is being returned.
    /// </returns>
    public async void SetupConnection(string username, string password, string hostname, int port, string bikeNr, bool sim, string vr)
    {
        _bikeNr = bikeNr;
        _sim = sim;
        _vrs = vr;
        Username = username;
        
        try
        {
            await Connect(hostname, port);
        }
        catch(Exception)
        {
            Callback.OnCallback(Error, "Kon geen verbinding maken met deze server.");
            return;
        }
        
        SendData(SendReplacedObject<string, JObject>("uuid", username, 1, SendReplacedObject<string,string>(
            "pass", password, 1, "application\\server\\init.json"
        )!)!);
        _stream!.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    /// <summary>
    /// It connects to the server, sends a request to create an account, and then waits for a response
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
            Callback.OnCallback(Error,"Kan niet verbinden met deze server");
            return;
        }
        
        SendData(SendReplacedObject("uuid", username, 1, SendReplacedObject("pass", password, 1, "application\\server\\createaccount.json"))!);
        _stream!.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    /// <summary>
    /// It creates a new TCP client, connects to the specified hostname and port, and then gets the stream from the client
    /// </summary>
    /// <param name="hostname">The hostname of the server you want to connect to.</param>
    /// <param name="port">The port to connect to.</param>
    private async Task Connect(string hostname, int port)
    {
        _client = new TcpClient();
        await _client.ConnectAsync(hostname, port);
        _stream = _client.GetStream();
    }

    /// <summary>
    /// It starts a connection with the VR server, waits until the connection is set, then sets up the hardware and sends a
    /// message to the VR server that the client is ready
    /// </summary>
    public async void SetupRest()
    {
        _isActive = true;
        
        await _vr.StartConnection(_vrs);
        
        while (!_vr.IsSet)
        {
            
        }

        Console.WriteLine("done with vr");
        SetupHardware(this, _bikeNr, _sim);
        SendData(SendReplacedObject("uuid", Username, 1, "application\\server\\clientready.json")!);
    }

    /// <summary>
    /// It closes the stream and the client
    /// </summary>
    public void SelfDestruct()
    {
        try
        {
            _stream?.Close(1000);
            _client?.Close();
        }
        catch (Exception) 
        {
            Console.WriteLine("Could not close stream properly");
        }
    }

    /// <summary>
    /// It sends bike data to the server
    /// </summary>
    /// <param name="values">The values from the bike.</param>
    /// <returns>
    /// A JObject
    /// </returns>
    public void OnNewBikeData(IReadOnlyList<int> values)
    {
        if(!ConnectedToServer) return;
        if (!_vr.IsSet) return;
        double speedAvg = -1;
        int heartrateAvg = -1;
        int time = -1;
        int distance = -1;
        
        double speed = Math.Round((values[8] + values[9] * 255) * 0.001 * 3.6, 1, MidpointRounding.AwayFromZero);
        _vr.UpdateBikeSpeed(speed);

        if (!IsSubscribed) return;

        if (SessionIsActive)
        {
            CollectedSpeeds.Add(speed);
            foreach (double s in CollectedSpeeds)
            {
                speedAvg += s;
            }
            speedAvg = Math.Round(speedAvg / CollectedSpeeds.Count, 1, MidpointRounding.AwayFromZero);
        
            CollectedRates.Add(_lastHeartrateData);
            foreach (int s in CollectedRates)
            {
                heartrateAvg += s;
            }
            heartrateAvg /= CollectedRates.Count;

            distance = values[7];
            if (PrevDistance > distance)
            {
                _distanceLoopCounter++;
            }
            PrevDistance = distance;
            distance = distance + 255 * _distanceLoopCounter - _distanceCorrection;

            time = Time;
        }
        else
        {
            _distanceCorrection = values[7];
        }

        JObject o = SendReplacedObject("client", Doctor, 1, SendReplacedObject(
            "speed", speed, 2, SendReplacedObject(
                "heartrate", _lastHeartrateData, 2, SendReplacedObject(
                    "distance", distance, 2, SendReplacedObject(
                        "time", time, 2, SendReplacedObject(
                            "uuid", Username, 1, SendReplacedObject(
                                "speedavg", speedAvg, 2, SendReplacedObject(
                                    "avgheartrate", heartrateAvg, 2, "application\\doctor\\senddata.json"
                                )
                            )
                        )
                    )
                )
            )
        ))!;
        SendData(o);
    }

    /// <summary>
    /// > If the server is connected and the VR headset is set, update the panel with the new heartrate data
    /// </summary>
    /// <param name="values">A list of ints, where the first value is the timestamp and the second value is the
    /// heartrate.</param>
    /// <returns>
    /// The heartrate data is being returned.
    /// </returns>
    public void OnNewHeartrateData(IReadOnlyList<int> values)
    {
        if(!ConnectedToServer) return;
        if (!_vr.IsSet) return;
        
        _vr.UpdatePanel(values[1]);
        _lastHeartrateData = values[1];
    }

    /// <summary>
    /// > When the connection is successful, call the callback function with the success parameter
    /// </summary>
    public void OnSuccessfulConnect()
    {
        Callback.OnCallback(Success);
    }

    /// <summary>
    /// It stops the client and sends a disconnect message to the server
    /// </summary>
    /// <param name="notify">If true, the server will send a message to all other clients that this client has
    /// disconnected.</param>
    /// <returns>
    /// A string
    /// </returns>
    public void Stop(bool notify)
    {
        if (!_isActive) return;
        new Thread(_vr.Stop).Start();
        HardwareConnector.Stop();
        SessionIsActive = false;
        ConnectedToServer = false;
        IsSubscribed = false;
        
        try
        {
            SendData(SendReplacedObject("client", Username, 1, SendReplacedObject("notify", notify, 1, "application\\server\\disconnect.json"))!);
        }
        catch (Exception)
        {
            Console.WriteLine("No connection with this server available");
        }
        
        _isActive = false;
    }

    /// <summary>
    /// > Send a JSON object to the client with the doctor's UUID, the client's UUID, and the status of the subscription
    /// </summary>
    /// <param name="doctor">The doctor's uuid</param>
    /// <param name="client">The doctor's uuid</param>
    /// <param name="status">0 - unsubscribed, 1 - subscribed</param>
    public void Subscribed(string doctor, string client, int status)
    {
        SendData(SendReplacedObject("client", doctor, 1, SendReplacedObject(
    /// <summary>
    /// The function takes a string as an argument, sets the current message to the string, calls the callback function, and
    /// then waits for 8 seconds before setting the current message to an empty string
    /// </summary>
    /// <param name="message">The message to send to the doctor</param>
            "uuid", client, 1, SendReplacedObject(
                "status", status, 1, "application\\doctor\\subscribed.json"
            )
        ))!);
    }

    public void SendDoctorMessage(string message)
    {
        _vr.CurrentMessage = message;
    /// <summary>
    /// It reads the data from the stream, decrypts it, and then calls the appropriate command handler
    /// </summary>
    /// <param name="IAsyncResult">The result of the asynchronous operation.</param>
        Callback.OnCallback(Chat, message + "\n");
        Thread.Sleep(8000);
        _vr.CurrentMessage = "";
    }

    private void OnRead(IAsyncResult ar)
    {
        try
        {
            int rc = _stream!.EndRead(ar);
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
            Stop(false);
        }
    }

    /// <summary>
    /// It takes a JObject, encrypts it, and sends it to the server
    /// </summary>
    /// <param name="JObject">This is the message that you want to send.</param>
    private void SendData(JObject message)
    {
        byte[] encryptedMessage = GetEncryptedMessage(message);
        try
        {
            _stream!.Write(encryptedMessage, 0, encryptedMessage.Length);
        }
        catch (Exception)
        {
            Stop(false);
        }
    }

    /// <summary>
    /// It adds all the commands to the dictionary
    /// </summary>
    private void InitCommands()
    {
        _commands.Add("client/server-connected", new ServerConnected());
        _commands.Add("client/startsession", new StartSession());
        _commands.Add("client/stopsession", new StopSession());
        _commands.Add("client/emergencystop", new EmergencyStop());
        _commands.Add("client/doctormessage", new DoctorMessage());
        _commands.Add("client/setresistance", new SetResistance());
        _commands.Add("client/account-created", new AccountCreated());
        _commands.Add("client/subscribe", new DoctorSubscribe());
        _commands.Add("client/unsubscribe", new DoctorUnsubscribe());
    }
}