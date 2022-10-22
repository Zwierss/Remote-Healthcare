using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.client;

public class ReceivedData : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Switch.SendToClient(packet, parent);
    }
}