using Newtonsoft.Json.Linq;
using static ClientApplication.State;

namespace ClientApplication.commandhandlers.server;

public class ServerConnected : ICommand
{
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