using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class AccountCreated : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        int status = packet["data"]!["status"]!.ToObject<int>();
        if (status == 1)
        {
            parent.ViewModel.OnChangedValues(Success);
        }
        else if (status == 2)
        {
            parent.ViewModel.OnChangedValues(Error, "Er bestaat al een gebruiker met deze gebruikersnaam.");
        }

        parent.SelfDestruct();
    }
}