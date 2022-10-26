using System.Diagnostics;
using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ServerConnected : ICommand
{
    /// <summary>
    /// It checks if the login was successful, and if so, it will call the OnChangedValues function of the ViewModel, which
    /// will change the view to the next view
    /// </summary>
    /// <param name="JObject">The packet that was received from the server.</param>
    /// <param name="DoctorClient">The client that sent the packet</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        int approved = packet["data"]["status"].ToObject<int>();
        if (approved == 1)
        {
            parent.ViewModel.OnChangedValues(Success);
        }
        else if(approved == 0)
        {
            parent.ViewModel.OnChangedValues(Error,new[]{"Deze combinatie van gebruikersnaam en wachtwoord bestaat niet." });
            parent.SelfDestruct();
        }
        else if(approved == 2)
        {
            parent.ViewModel.OnChangedValues(Error, new[] { "Dit account is al ingelogd op deze server" });
            parent.SelfDestruct();
        }
    }
}