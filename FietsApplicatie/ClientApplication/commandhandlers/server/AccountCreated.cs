using Newtonsoft.Json.Linq;
using static ClientApplication.State;

namespace ClientApplication.commandhandlers.server;

public class AccountCreated : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        int status = packet["data"]!["status"]!.ToObject<int>();
        if (status == 1)
        {
            parent.Callback.OnCallback(Success);
        }
        else if (status == 2)
        {
            parent.Callback.OnCallback(Error, "Er bestaat al een gebruiker met deze gebruikersnaam.");
        }

        parent.SelfDestruct();
    }
}