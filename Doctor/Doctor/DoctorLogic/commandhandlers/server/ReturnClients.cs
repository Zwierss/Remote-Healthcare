using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnClients : ICommand
{
    /// <summary>
    /// It takes a packet, and a parent, and then it gets the clients from the packet, and then it calls the OnChangedValues
    /// function on the parent's ViewModel, passing in the Replace function, and then it calls the OnChangedValues function
    /// on the parent's ViewModel, passing in the Store function and the clients
    /// </summary>
    /// <param name="JObject">The packet that was received from the server.</param>
    /// <param name="DoctorClient">The client that sent the command.</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        string[] clients = packet["data"]!["clients"]!.ToObject<string[]>()!;
        parent.ViewModel.OnChangedValues(Replace);
        parent.ViewModel.OnChangedValues(Store,clients);
    }
}