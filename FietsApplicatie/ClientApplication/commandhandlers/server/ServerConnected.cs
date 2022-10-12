using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.server;

public class ServerConnected : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Console.WriteLine("Er is verbonden met de server!");
    }
}