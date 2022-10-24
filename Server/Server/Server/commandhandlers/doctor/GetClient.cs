using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.doctor;

public class GetClient : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        List<string> clientUuids = new();
        foreach (Client c in parent.Parent.Clients)
        {
            if (!c.IsDoctor)
            {
                clientUuids.Add(c.Uuid);
            }
        }
        
        parent.SendMessage(PacketSender.SendReplacedObject("clients", clientUuids, 1, "doctor\\returnclients.json")!);
    }
}