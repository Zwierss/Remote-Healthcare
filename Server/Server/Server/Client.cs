using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using static Server.Cryptographer;

namespace Server;

public class Client
{

    private TcpClient _tcp;
    private NetworkStream _stream;
    private readonly Dictionary<string, ICommand> _commands;

    public event EventHandler<byte[]>? OnMessage; 
    
    public Client(TcpClient tcp)
    {
        _tcp = tcp;
        _stream = _tcp.GetStream();
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        OnMessage += (_, data) => MessageReceived(data);
    }

    public void SendMessage(JObject packet)
    {
        byte[] encryptedMessage = GetEncryptedMessage(packet);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    public void MessageReceived(byte[] message)
    {
        Console.WriteLine("test");
        JObject packet = GetDecryptedMessage(message);
        Console.WriteLine(packet);
        
        try
        {
            _commands[packet["id"]!.ToObject<string>()!].OnCommandReceived(packet,this);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void InitCommands()
    {
        _commands.Add("server/test", new TestCommand());
    }
}