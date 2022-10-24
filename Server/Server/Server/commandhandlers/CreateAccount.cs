using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class CreateAccount : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string uuid = packet["data"]!["uuid"]!.ToObject<string>()!;
        string pass = packet["data"]!["pass"]!.ToObject<string>()!;
        bool isDoctor = packet["data"]!["doctor"]!.ToObject<bool>()!;
        string type = isDoctor ? "doctors" : "clients";

        if (StorageManager.CheckIfNewUsername(uuid, type))
        {
            StorageManager.AddNewAccount(uuid,pass,type);
            parent.SendMessage(PacketSender.SendReplacedObject("status", 1, 1, "accountcreated.json")!);
        }
        else
        {
            parent.SendMessage(PacketSender.SendReplacedObject("status", 0, 1, "accountcreated.json")!);
        }
        parent.SelfDestruct();
    }
}