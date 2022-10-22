using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class ServerConnected : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        parent.SendData(PacketSender.GetJson("server\\getclients.json"));
    }
}