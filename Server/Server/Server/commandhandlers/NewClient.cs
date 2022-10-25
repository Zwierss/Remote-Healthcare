using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class NewClient : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string uuid = packet["data"]!["uuid"]!.ToObject<string>()!;
        bool isDoctor = packet["data"]!["doctor"]!.ToObject<bool>();

        bool exists;
        if (isDoctor)
        {
            exists = parent.StorageManager.CheckIfAccountExists(uuid, packet["data"]!["pass"]!.ToObject<string>()!, "doctors");
        }
        else
        {
            exists = parent.StorageManager.CheckIfAccountExists(uuid, packet["data"]!["pass"]!.ToObject<string>()!, "clients");
        }

        bool alreadyOpen = parent.StorageManager.CheckIfAlreadyOpen(uuid, parent.Parent.Clients);
            
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
        
        parent.Uuid = uuid;
        parent.IsDoctor = isDoctor;
    }
}