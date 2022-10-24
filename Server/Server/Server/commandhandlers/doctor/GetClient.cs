using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.doctor;

public class GetClient : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SendClientList();
    }
}