using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class Switch : ICommand
{
    /// <summary>
    /// If the client is not the receiver, send the packet to the receiver
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the message</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string receiver = packet["data"]!["client"]!.ToObject<string>()!;
        foreach (Client c in parent.Parent.Clients)
        {
            if (c.Uuid != receiver) continue;
            c.SendMessage(packet);
            break;
        }
    }
}