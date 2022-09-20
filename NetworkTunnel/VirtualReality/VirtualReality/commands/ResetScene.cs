using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class ResetScene : Command
{
    public void OnCommandReceived(JObject ob, Client client)
    {
        Console.WriteLine("received shit back");
    }
}