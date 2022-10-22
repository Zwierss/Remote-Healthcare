using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.doctor;

public class EmergencyStop : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Switch.SendToClient(packet, parent);
    }
}