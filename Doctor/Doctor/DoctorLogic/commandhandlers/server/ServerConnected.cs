using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class ServerConnected : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        bool approved = packet["data"]!["status"]!.ToObject<bool>();
        parent.LoginSuccessful = approved;
        if (approved)
        {
            parent.SendData(PacketSender.GetJson("server\\getclients.json"));
        }
    }
}