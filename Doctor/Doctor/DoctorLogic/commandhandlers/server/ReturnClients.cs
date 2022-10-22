using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class ReturnClients : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        
    }
}