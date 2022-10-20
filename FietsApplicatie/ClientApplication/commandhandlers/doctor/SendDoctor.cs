using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class SendDoctor : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        
    }
}