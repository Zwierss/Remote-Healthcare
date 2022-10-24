using System.Reflection.Emit;
using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class ReturnClient : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        string uuid = packet["data"]!["client"]!.ToObject<string>()!;
        parent.ViewModel.OnChangedValues(State.Store, new[]{uuid});
    }
}