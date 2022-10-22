using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public static class Switch
{
    public static void SendToClient(JObject packet, Client parent)
    {
        string receiver = packet["data"]!["receiver"]!.ToObject<string>()!;
        foreach (Client c in parent.Parent.Clients)
        {
            if (!string.IsNullOrEmpty(c.Uuid = receiver))
            {
                c.SendMessage(packet);
            }
        }
    }
}