using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.doctor;

public class StopSession : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string username = packet["data"]!["client"]!.ToObject<string>()!;
        
        foreach (Client client in parent.Parent.Clients)
        {
            if(client.Uuid != username)continue;
            client.StorageManager.SaveSession(client.CurrentSessionTitle);
            client.SessionActive = false;
        }
        new Switch().OnCommandReceived(packet, parent);
    }
}