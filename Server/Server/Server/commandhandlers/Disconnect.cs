using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class Disconnect : ICommand
{
    /// <summary>
    /// If the client is a doctor, send a list of all clients to the doctor
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command</param>
    /// <returns>
    /// A list of all the clients in the server.
    /// </returns>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        if (packet["data"]!["notify"]!.ToObject<bool>())
        {
            parent.SelfDestruct(true);
        }
        
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

        if (clientUuids.Count == 0 || doctors.Count == 0) return;
        foreach (Client d in doctors)
        {
            d.SendMessage(PacketSender.SendReplacedObject("clients", clientUuids, 1, "doctor\\returnclients.json")!);
        }
    }
}