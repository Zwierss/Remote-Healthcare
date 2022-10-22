using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.doctor;

public class ChangeResistance : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Switch.SendToClient(packet, parent);
    }
}