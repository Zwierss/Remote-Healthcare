using System.Diagnostics;
using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ServerConnected : ICommand
{
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
    }
}