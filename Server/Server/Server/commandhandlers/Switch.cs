using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public static class Switch
{
    public static void SendToClient(JObject packet, Client parent)
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