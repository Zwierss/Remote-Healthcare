using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.server;

public class ServerReceived : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        
    }
}