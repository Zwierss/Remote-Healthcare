using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class Disconnect : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        List<string> clientUuids = new();
        List<Client> doctors = new();
        
        foreach (Client c in parent.Parent.Clients)
        {
            if (c.IsDoctor)
            {
                doctors.Add(c);
            }
            else
            {
                clientUuids.Add(c.Uuid);
            }
        }

        foreach (Client d in doctors)
        {
            d.SendMessage(PacketSender.SendReplacedObject("clients", clientUuids, 1, "doctor\\returnclients.json")!);
        }
        
        parent.SendMessage(PacketSender.GetJson("disconnected.json"));
        parent.SelfDestruct();
    }
}