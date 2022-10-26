using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.doctor;

public class GetClient : ICommand
{
    /// <summary>
    /// It gets a list of all the clients that are online, and sends it to the client
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        List<string> clientUuids = new();
        foreach (Client c in parent.Parent.OnlineClients)
        {
            clientUuids.Add(c.Uuid);
        }
        
        parent.SendMessage(PacketSender.SendReplacedObject("clients", clientUuids, 1, "doctor\\returnclients.json")!);
    }
}