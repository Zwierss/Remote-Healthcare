using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnClients : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        string[] clients = packet["data"]!["clients"]!.ToObject<string[]>()!;
        parent.ViewModel.OnChangedValues(Replace);
        parent.ViewModel.OnChangedValues(Store,clients);
    }
}