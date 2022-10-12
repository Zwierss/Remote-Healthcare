using Newtonsoft.Json.Linq;

namespace Server;

public class TestCommand : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Console.WriteLine(packet);
    }
}