using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class Switch : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string receiver = packet["data"]!["client"]!.ToObject<string>()!;
        foreach (Client c in parent.Parent.Clients)
        {
            if (c.Uuid != receiver) continue;
            c.SendMessage(packet);
            break;
        }
    }
}