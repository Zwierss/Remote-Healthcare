using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.client;

public class ServerConnected : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.ConnectedToServer = true;
    }
}