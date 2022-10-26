using System.Reflection.Emit;
using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class ReturnClient : ICommand
{
    /// <summary>
    /// "When a command is received, update the store with the new values."
    /// 
    /// The first line of the function is a bit of a mouthful, but it's just a way of getting the UUID of the client that
    /// sent the command
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the server.</param>
    /// <param name="DoctorClient">The client that received the command.</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        string uuid = packet["data"]!["client"]!.ToObject<string>()!;
        parent.ViewModel.OnChangedValues(State.Store, new[]{uuid});
    }
}