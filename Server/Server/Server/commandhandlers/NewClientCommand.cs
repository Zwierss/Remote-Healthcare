using Newtonsoft.Json.Linq;

namespace Server.commandhandlers;

public class NewClientCommand : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        try
        {
            parent.IsDoctor = packet["data"]!["doctor"]!.ToObject<bool>();
        }
        catch(Exception)
        {
            Console.WriteLine("Could not find such data in this packet");
        }
    }
}