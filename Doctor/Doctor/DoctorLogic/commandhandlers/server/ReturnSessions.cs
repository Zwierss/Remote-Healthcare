using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnSessions : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        string[] args = packet["data"]!["files"]!.ToObject<string[]>()!;
        Console.WriteLine(args[0]);
        parent.ViewModel.OnChangedValues(Data, args);
    }
}