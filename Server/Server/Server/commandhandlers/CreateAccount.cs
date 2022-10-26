using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class CreateAccount : ICommand
{
    /// <summary>
    /// It checks if the username is new, if it is, it adds it to the database, and sends a message to the client saying
    /// that the account was created
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string uuid = packet["data"]!["uuid"]!.ToObject<string>()!;
        string pass = packet["data"]!["pass"]!.ToObject<string>()!;
        bool isDoctor = packet["data"]!["doctor"]!.ToObject<bool>()!;
        string type = isDoctor ? "doctors" : "clients";
        
        if (parent.StorageManager.CheckIfNewUsername(uuid))
        {
            parent.StorageManager.AddNewAccount(uuid,pass,type);
            parent.SendMessage(PacketSender.SendReplacedObject("status", 1, 1, "accountcreated.json")!);    
        }
        else
        {
            parent.SendMessage(PacketSender.SendReplacedObject("status", 0, 1, "accountcreated.json")!);
        }
        parent.SelfDestruct(true);
    }
}