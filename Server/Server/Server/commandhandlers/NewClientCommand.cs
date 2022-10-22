using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class NewClientCommand : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SendMessage(PacketSender.GetJson("serverconnected.json"));
        parent.Uuid = packet["data"]!["uuid"]!.ToObject<string>()!;
    }
}