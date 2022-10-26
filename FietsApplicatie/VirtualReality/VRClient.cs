using System.Net.Sockets;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtualReality.commands;
using VirtualReality.commands.tunnel;
using VirtualReality.components;

namespace VirtualReality;

public class VRClient
{
    private TcpClient _client = null!;
    private NetworkStream _stream = null!;

    private readonly Dictionary<string, ICommand> _commands;
    
    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public string? TunnelId { get; set; }
    public string? TerrainId { get; set; }
    public string? RouteId { get; set; }
    public string? CameraId { get; set; }
    public float[] Heights { get; set; }
    public string? BikeId { get; set; }
    public string? HeadId { get; set; }
    public string? PanelId { get; set; }
    public bool IsSet { get; set; }
    public string CurrentMessage { get; set; }

    private const string Hostname = "145.48.6.10";
    private const int Port = 6666;

    private bool _tunnelCreated;
    private bool _stopRunning;
    private double _currentSpeed;
    private bool _isActive;
    
    private readonly HeightMap _map;
    private readonly Route _route;
    private readonly Bike _bike;
    private readonly Camera _camera;
    private readonly Tree _tree;
    private readonly Panel _panel;
    private readonly House _house;
    
    /* This is the constructor of the VRClient class. It initializes all the variables and creates the commands dictionary. */
    public VRClient()
    {
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        _tunnelCreated = false;
        _map = new HeightMap(this);
        _route = new Route(this);
        _bike = new Bike(this);
        _camera = new Camera(this);
        _tree = new Tree(this);
        _house = new House(this);
        _panel = new Panel(this);
        Heights = new float[200];
        IsSet = false;
        _currentSpeed = 0;
        CurrentMessage = "";
        _isActive = false;
    }

    /// <summary>
    /// It creates a new TCP client, connects to the server, gets the stream, sends the sessionlist.json packet, and then
    /// begins reading the stream
    /// </summary>
    public async Task StartConnection()
    {
        _stopRunning = false;
        _isActive = true;
        
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(Hostname, Port);
            _stream = _client.GetStream();
            SendData(PacketSender.GetJson("sessionlist.json"));
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// It takes a JObject, converts it to a string, converts the string to a byte array, and sends it to the server
    /// </summary>
    /// <param name="JObject">The data you want to send.</param>
    public void SendData(JObject o)
    {
        string message = o.ToString();
        byte[] requestLength = BitConverter.GetBytes(message.Length);
        byte[] request = Encoding.ASCII.GetBytes(message);
        try
        {
            _stream.Write(requestLength, 0, requestLength.Length);
            _stream.Write(request, 0, request.Length);
        }
        catch (Exception)
        {
            Stop();
        }
    }
    
    /// <summary>
    /// It sends a message to the server, which then sends it to the other client
    /// </summary>
    /// <param name="tunnelId">The id of the tunnel you want to send data to.</param>
    /// <param name="dynamic">This is the data you want to send to the client.</param>
    public void SendTunnel(string tunnelId, dynamic jsonData)
    {
        var command = new { id = "tunnel/send", data = (dynamic)new { dest = TunnelId, data = new { id = tunnelId, data = jsonData } } };
        byte[] d = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
        try
        {
            _stream.WriteAsync(BitConverter.GetBytes(d.Length), 0, 4).Wait();
            _stream.WriteAsync(d, 0, d.Length).Wait();
        }
        catch (Exception)
        {
            Stop();
        }
    }

    /// <summary>
    /// > This function sets the tunnel id and sets the tunnel created flag to true
    /// </summary>
    /// <param name="id">The id of the tunnel you want to connect to.</param>
    public void SetTunnel(string id)
    {
        TunnelId = id;
        _tunnelCreated = true;
    }

    /// <summary>
    /// It creates a tunnel between the client and the server.
    /// </summary>
    /// <param name="id">The id of the session you want to create a tunnel for.</param>
    public void CreateTunnel(string id)
    {
        SendData(PacketSender.SendReplacedObject<string,string>("session", id, 1, "createtunnel.json")!);
    }

    /// <summary>
    /// It reads the data from the server and then checks if the data is a command. If it is, it will execute the command
    /// </summary>
    /// <param name="IAsyncResult">The result of the asynchronous operation.</param>
    /// <returns>
    /// The data that is being returned is the data that is being sent from the server.
    /// </returns>
    private void OnRead(IAsyncResult ar)
    {
        try
        {
            int rc = _stream.EndRead(ar);
            _totalBuffer = Concat(_totalBuffer, _buffer, rc);
        }
        catch(Exception)
        {
            return;
        }
        
        while (_totalBuffer.Length >= 4)
        {
            try
            {
                int packetSize = BitConverter.ToInt32(_totalBuffer, 0);
                if (_totalBuffer.Length >= packetSize + 4)
                {
                    string data = Encoding.UTF8.GetString(_totalBuffer, 4, packetSize);
                    JObject jData = JObject.Parse(data);
                
                    if(_commands.ContainsKey(jData["id"]!.ToObject<string>()!))
                        _commands[jData["id"]!.ToObject<string>()!].OnCommandReceived(jData, this);
                
                    var newBuffer = new byte[_totalBuffer.Length - packetSize - 4];
                    Array.Copy(_totalBuffer, packetSize + 4, newBuffer, 0, newBuffer.Length);
                    _totalBuffer = newBuffer;
                }
                else
                    break;
            }
            catch (Exception)
            {
                break;
            }
        }

        try
        {
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch (Exception)
        {
            return;
        }

        if (!_tunnelCreated) return;
        
        _tunnelCreated = false;
        _map.RenderHeightMap();
        _route.CreateRoute();
        _bike.PlaceBike();
        _camera.SetView();
        _panel.AddPanel();
        _tree.PlaceTrees();
        _house.PlaceHouses();
        IsSet = true;
    }

    /// <summary>
    /// It sends a JSON object to the server, which is then sent to the bike
    /// </summary>
    /// <param name="speed">The speed of the bike in km/h</param>
    /// <returns>
    /// A JObject
    /// </returns>
    public void UpdateBikeSpeed(double speed)
    {
        if(_stopRunning) return;
        
        SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,JObject>(
            "node", BikeId, 1, PacketSender.SendReplacedObject<double,string>(
                "speed", speed, 1, "route\\speedfollowroute.json"
            )!
        )!, TunnelId!)!);

        _currentSpeed = speed;
    }

    /// <summary>
    /// This function updates the panel with the current speed, heart rate, and message
    /// </summary>
    /// <param name="heartRate">The current heart rate of the player.</param>
    public void UpdatePanel(double heartRate)
    {
        _panel.UpdatePanel(_currentSpeed, heartRate, CurrentMessage);
    }

    /// <summary>
    /// It takes two byte arrays and a count, and returns a new byte array that is the concatenation of the first two
    /// arrays, with the second array truncated to the specified count
    /// </summary>
    /// <param name="b1">The first byte array to concatenate.</param>
    /// <param name="b2">The array to copy from</param>
    /// <param name="count">The number of bytes to copy from the second array.</param>
    /// <returns>
    /// <summary>
    /// It adds two commands to the dictionary
    /// </summary>
    /// A byte array
    /// </returns>
    public static byte[] Concat(byte[] b1, byte[] b2, int count)
    {
        byte[] r = new byte[b1.Length + count];
        Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
        Buffer.BlockCopy(b2, 0, r, b1.Length, count);
        return r;
    }

    private void InitCommands()
    {
        _commands.Add("session/list", new SessionListCommand());
        _commands.Add("tunnel/create", new CreateTunnelCommand());
        _commands.Add("tunnel/send", new TunnelCommand());
    }

    /// <summary>
    /// The function stops the bike by setting the speed to 0, closing the connection to the server, and closing the stream
    /// </summary>
    /// <returns>
    /// The method is returning the current speed of the bike.
    /// </returns>
    public void Stop()
    {
        if(!_isActive) return;
        
        UpdateBikeSpeed(0.0);
        _stopRunning = true;
        IsSet = false;
        _isActive = false;
        SendData(PacketSender.GetJson("scene\\resetscene.json"));
        Thread.Sleep(2000);
        _stream.Close(400);
        _client.Close();
    }
}