using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.doctor;

public class StopSession : ICommand
{
    /// <summary>
    /// It saves the session of the client that is being switched to, and then switches to the client
    /// </summary>
    /// <param name="JObject">The packet that was received from the client.</param>
    /// <param name="Client">The client that sent the command</param>
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