using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class NewClient : ICommand
{
    /// <summary>
    /// If the account exists, and the account isn't already open, then send a status of 1, otherwise send a status of 0 or
    /// 2
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the packet</param>
    /// <returns>
    /// A JObject
    /// </returns>
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
            
        if (!exists)
        {
            parent.SendMessage(PacketSender.SendReplacedObject("status", 0, 1, "serverconnected.json")!);
            parent.SelfDestruct(true);
            return;
            
        }
        if (!alreadyOpen)
        {
            parent.SendMessage(PacketSender.SendReplacedObject("status", 2, 1, "serverconnected.json")!);
            parent.SelfDestruct(true);
            return;
        }
        
        parent.SendMessage(PacketSender.SendReplacedObject("status", 1, 1, "serverconnected.json")!);
        
        parent.Uuid = uuid;
        parent.IsDoctor = isDoctor;
    }
}