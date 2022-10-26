using Newtonsoft.Json.Linq;
using static ClientApplication.State;

namespace ClientApplication.commandhandlers.server;

public class ServerConnected : ICommand
{
    /// <summary>
    /// If the status is 1, the user is logged in, if the status is 0, the user doesn't exist, if the status is 2, the user
    /// is already logged in
    /// </summary>
    /// <param name="JObject">The packet that was received</param>
    /// <param name="Client">The client that received the packet</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        int status = packet["data"]!["status"]!.ToObject<int>();
        if (status == 1)
        {
            parent.ConnectedToServer = true;
            new Thread(parent.SetupRest).Start();
        }
        else if (status == 0)
        {
            parent.Callback.OnCallback(Error, "Deze combinatie van wachtwoord en gebruikersnaam bestaat niet");
            parent.SelfDestruct();
        }
        else if (status == 2) 
        {
            parent.Callback.OnCallback(Error, "Deze gebruiker is al ingelogd");
            parent.SelfDestruct();
        }
    }
}