using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnOffline : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        string[] args = packet["data"]!["names"]!.ToObject<string[]>()!;
        parent.ViewModel.OnChangedValues(Data, args);
    }
}