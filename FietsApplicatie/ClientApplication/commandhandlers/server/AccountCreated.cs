using Newtonsoft.Json.Linq;
using static ClientApplication.State;

namespace ClientApplication.commandhandlers.server;

public class AccountCreated : ICommand
{
    /// <summary>
    /// If the status is 1, the callback will be called with the Success function, if the status is 0, the callback will be
    /// called with the Error function and the message "Er bestaat al een gebruiker met deze gebruikersnaam."
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the packet.</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        int status = packet["data"]!["status"]!.ToObject<int>();
        if (status == 1)
        {
            parent.Callback.OnCallback(Success);
        }
        else if (status == 0)
        {
            parent.Callback.OnCallback(Error, "Er bestaat al een gebruiker met deze gebruikersnaam.");
        }

        parent.SelfDestruct();
    }
}