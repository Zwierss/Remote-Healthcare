using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class GetClientCommand : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        List<string> clientUuids = new();
        foreach (Client c in parent.Parent.Clients)
        {
            if ((bool)!c.IsDoctor)
            {
                clientUuids.Add(c.Uuid);
            }
        }
        
        parent.SendMessage(PacketSender.SendReplacedObject("clients", clientUuids, 1, "returnclients.json")!);
    }
}