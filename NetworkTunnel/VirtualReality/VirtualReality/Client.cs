using System.Net.Sockets;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtualReality.commands;
using VirtualReality.components;

namespace VirtualReality;

public class Client
{
    private TcpClient _client = null!;
    private NetworkStream _stream = null!;

    private readonly Dictionary<string, ICommand> _commands;
    
    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public string? TunnelId { get; set; }
    public string? NodeId { get; set; }
    public string? RouteId { get; set; }
    public float[] Heights { get; set; }

    private const string Hostname = "145.48.6.10";
    private const int Port = 6666;

    private bool _tunnelCreated;

    private readonly Skybox _skybox;
    private readonly HeightMap _map;
    private readonly Route _route;

    public Client()
    {
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        _tunnelCreated = false;
        _skybox = new Skybox(this);
        _map = new HeightMap(this);
        _route = new Route(this);
        Heights = new float[200];
    }

    public async Task StartConnection()
    {
        Console.WriteLine("Connecting to Server");
        
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(Hostname, Port);
            _stream = _client.GetStream();
            SendData(PacketSender.GetJson("sessionlist.json"));
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        _stream = _client.GetStream();
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    public void SendData(JObject o)
    {
        string message = o.ToString();
        Console.WriteLine("Sending message " + message);
        byte[] requestLength = BitConverter.GetBytes(message.Length);
        byte[] request = Encoding.ASCII.GetBytes(message);
        _stream.Write(requestLength, 0 , requestLength.Length);
        _stream.Write(request, 0, request.Length);
    }
    
    public void SendTunnel(string tunnelId, dynamic jsonData)
    {
        var command = new { id = "tunnel/send", data = (dynamic)new { dest = TunnelId, data = new { id = tunnelId, data = jsonData } } };
        Console.WriteLine("Sending message " + command);
        byte[] d = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
        _stream.WriteAsync(BitConverter.GetBytes(d.Length), 0, 4).Wait();
        _stream.WriteAsync(d, 0, d.Length).Wait();
    }

    public void SetTunnel(string id)
    {
        Console.WriteLine("Setting Tunnel ID");
        TunnelId = id;
        _tunnelCreated = true;
    }

    public void CreateTunnel(string id)
    {
        Console.WriteLine("Setting Tunnel");
        SendData(PacketSender.SendReplacedObject<string,string>("session", id, 1, "createtunnel.json")!);

        //SendData($@"{{""id"": ""tunnel/create"", ""data"":{{""session"":""{id}"", ""key"":""""}}}}");
        //SendData((JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText("JSON/createtunnel.json"))));
    }

    private void OnRead(IAsyncResult ar)
    {
        try
        {
            int rc = _stream.EndRead(ar);
            _totalBuffer = Concat(_totalBuffer, _buffer, rc);
        }
        catch(IOException)
        {
            Console.WriteLine("Error");
            return;
        }
        
        while (_totalBuffer.Length >= 4)
        {
            int packetSize = BitConverter.ToInt32(_totalBuffer, 0);
            if (_totalBuffer.Length >= packetSize + 4)
            {
                string data = Encoding.UTF8.GetString(_totalBuffer, 4, packetSize);
                JObject jData = JObject.Parse(data);
                
                if(_commands.ContainsKey(jData["id"]!.ToObject<string>()!))
                {
                    Console.WriteLine("Received Command " + jData);
                    _commands[jData["id"]!.ToObject<string>()!].OnCommandReceived(jData, this);
                }
                else
                {
                    Console.WriteLine($"Could not find command for {jData["id"]}");
                }
                var newBuffer = new byte[_totalBuffer.Length - packetSize - 4];
                Array.Copy(_totalBuffer, packetSize + 4, newBuffer, 0, newBuffer.Length);
                _totalBuffer = newBuffer;
            }
            else
                break;
        }
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);

        if (!_tunnelCreated) return;
        
        _tunnelCreated = false;
        _map.RenderHeightMap();
        _route.CreateRoute();
        new Thread(_skybox.Update).Start();
    }

    private static byte[] Concat(byte[] b1, byte[] b2, int count)
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
}