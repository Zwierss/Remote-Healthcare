using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class NewClientCommand : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Console.WriteLine(packet);
    }
}