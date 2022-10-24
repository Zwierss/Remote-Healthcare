using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class Disconnected : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        parent.SelfDestruct();
    }
}