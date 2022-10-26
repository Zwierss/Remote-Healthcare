using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnOffline : ICommand
{
    /// <summary>
    /// It takes a packet, and a parent, and then it gets the names of the changed values from the packet, and then it calls
    /// the parent's OnChangedValues function with the data and the names
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="DoctorClient">The client that sent the command.</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        string[] args = packet["data"]!["names"]!.ToObject<string[]>()!;
        parent.ViewModel.OnChangedValues(Data, args);
    }
}