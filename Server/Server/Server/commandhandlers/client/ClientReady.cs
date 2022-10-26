using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.client;

public class ClientReady : ICommand
{
    /// <summary>
    /// It adds the client to the list of online clients, and then sends a packet to all doctors saying that the client is
    /// online
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string uuid = packet["data"]!["uuid"]!.ToObject<string>()!;
        parent.Parent.OnlineClients.Add(parent);
        foreach (Client c in parent.Parent.Clients)
        {
            if(!c.IsDoctor) continue;
            c.SendMessage(PacketSender.SendReplacedObject("client", uuid, 1, "doctor//returnclient.json")!);
        }
    }
}