using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class NewClient : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string uuid = packet["data"]!["uuid"]!.ToObject<string>()!;
        bool isDoctor = packet["data"]!["doctor"]!.ToObject<bool>();

        if (isDoctor)
        {
            bool exists = StorageManager.CheckIfDoctorExists(uuid, packet["data"]!["pass"]!.ToObject<string>()!);
            bool alreadyOpen = StorageManager.CheckIfAlreadyOpen(uuid, parent.Parent.Clients);
            
            if (exists && alreadyOpen)
            {
                parent.SendMessage(PacketSender.SendReplacedObject("status", 1, 1, "serverconnected.json")!);
            }
            else
            {
                parent.SendMessage(PacketSender.SendReplacedObject("status", 0, 1, "serverconnected.json")!);
                parent.SelfDestruct();
                return;
            }
        }
        
        parent.Uuid = uuid;
        parent.IsDoctor = isDoctor;

        if(parent.Parent.Clients.Count == 0) return;
        if(isDoctor) return;
        foreach (Client c in parent.Parent.Clients)
        {
            if(!c.IsDoctor) continue;
            c.SendMessage(PacketSender.SendReplacedObject("client", uuid, 1, "doctor\\returnclient.json")!);
        }
    }
}