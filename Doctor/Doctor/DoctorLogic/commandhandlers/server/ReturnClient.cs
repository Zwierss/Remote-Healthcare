using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class ReturnClient : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        
    }
}