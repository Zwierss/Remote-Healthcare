using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.client;

public class ReceivedData : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        
    }
}