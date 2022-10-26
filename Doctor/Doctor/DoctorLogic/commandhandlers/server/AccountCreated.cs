using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class AccountCreated : ICommand
{
    /// <summary>
    /// If the status is 1, the user was successfully created, if the status is 0, the username already exists
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="DoctorClient">The client that sent the command.</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        int status = packet["data"]!["status"]!.ToObject<int>();
        if (status == 1)
        {
            parent.ViewModel.OnChangedValues(Success);
        }
        else if (status == 0)
        {
            parent.ViewModel.OnChangedValues(Error, new[]{"Er bestaat al een gebruiker met deze gebruikersnaam."});
        }

        parent.SelfDestruct();
    }
}