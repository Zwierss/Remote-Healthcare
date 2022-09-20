using System.Net.Sockets;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class Client
{
    private TcpClient _client;
    private NetworkStream _stream;

    private readonly Dictionary<string, Command> _commands;
    
    private byte[] _totalBuffer = new byte[0];
    private readonly byte[] _buffer = new byte[1024];

    private string? _tunnelID = null;

    private static Client? _instance;

    private static string _HOSTNAME = "145.48.6.10";
    private static int _PORT = 6666;

    public Client()
    {
        _commands = new Dictionary<string, Command>();
        InitCommands();
    }

    public async Task StartConnection()
    {
        Console.WriteLine("Connecting to Server");
        
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(_HOSTNAME, _PORT);
            _stream = _client.GetStream();
            SendData((JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText("JSON/sessionlist.json"))));
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

    public void SetTunnel(string id)
    {
        Console.WriteLine("Setting Tunnel ID");
        _tunnelID = id;
    }

    public void CreateTunnel(string id)
    {
        Console.WriteLine("Setting Tunnel");
        SendData(PacketHandler.ReplaceObject("session", id, 1, "createtunnel.json"));

        //SendData($@"{{""id"": ""tunnel/create"", ""data"":{{""session"":""{id}"", ""key"":""""}}}}");
        //SendData((JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText("JSON/createtunnel.json"))));
    }

    public void OnRead(IAsyncResult ar)
    {
        Console.WriteLine("Method Checked");
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
        
        Console.WriteLine("Didnt get returned");
        while (_totalBuffer.Length >= 4)
        {
            int packetSize = BitConverter.ToInt32(_totalBuffer, 0);
            if (_totalBuffer.Length >= packetSize + 4)
            {
                string data = Encoding.UTF8.GetString(_totalBuffer, 4, packetSize);
                JObject jData = JObject.Parse(data);
                
                if(_commands.ContainsKey(jData["id"].ToObject<string>()))
                {
                    Console.WriteLine("Received Command " + jData);
                    _commands[jData["id"].ToObject<string>()].OnCommandReceived(jData, this);
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
        Console.WriteLine("Begin Read");
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
        _commands.Add("session/list", new SessionList());
        _commands.Add("tunnel/create", new CreateTunnel());
    }
}