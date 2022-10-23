using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.server;

public class Disconnected : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SelfDestruct();
    }
}