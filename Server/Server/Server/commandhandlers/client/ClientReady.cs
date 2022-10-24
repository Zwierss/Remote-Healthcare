using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.client;

public class ClientReady : ICommand
{
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