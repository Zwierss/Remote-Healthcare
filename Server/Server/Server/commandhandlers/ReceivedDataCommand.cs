using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class ReceivedDataCommand : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        
    }
}