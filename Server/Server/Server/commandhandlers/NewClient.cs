using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class NewClient : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SendMessage(PacketSender.GetJson("serverconnected.json"));

        string uuid = packet["data"]!["uuid"]!.ToObject<string>()!;
        parent.Uuid = uuid;
        
        bool isDoctor =  packet["data"]!["doctor"]!.ToObject<bool>();
        parent.IsDoctor = isDoctor;

        if (isDoctor) return;
        
        foreach (Client c in parent.Parent.Clients)
        {
            if((bool)!c.IsDoctor) return;
            c.SendMessage(PacketSender.SendReplacedObject("client", uuid, 1, "doctor\\returnclient.json")!);
        }
    }
}