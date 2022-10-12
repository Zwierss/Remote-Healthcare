using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.server;

public class ServerLogin : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        try
        {
            if (packet["newAccount"]!.ToObject<bool>())
            {
                Console.WriteLine("Nieuw Account aangemaakt");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("No such thing found as newAccount");
        }
    }
}