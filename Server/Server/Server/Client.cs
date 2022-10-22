﻿using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using Server.commandhandlers;
using static Server.Cryptographer;

namespace Server;

public class Client
{

    private readonly TcpClient _tcp;
    private readonly NetworkStream _stream;
    private readonly Dictionary<string, ICommand> _commands;
    private MainServer _parent;
    
    public bool? IsDoctor { get; set; }
    public string Uuid { get; set; }

    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public Client(TcpClient tcp, MainServer parent)
    {
        _parent = parent;
        _tcp = tcp;
        _stream = _tcp.GetStream();
        _commands = new Dictionary<string, ICommand>();
        InitCommands();
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    public void SendMessage(JObject packet)
    {
        byte[] encryptedMessage = GetEncryptedMessage(packet);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
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
            Console.WriteLine("Can no longer read from this client");
            _parent.Clients.Remove(this);
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

        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    private void InitCommands()
    {
        _commands.Add("server/client-enter", new NewClientCommand());
        _commands.Add("server/client-senddata", new ReceivedDataCommand());
    }
    
    private static byte[] Concat(byte[] b1, byte[] b2, int count)
    {
        byte[] r = new byte[b1.Length + count];
        Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
        Buffer.BlockCopy(b2, 0, r, b1.Length, count);
        return r;
    }
}