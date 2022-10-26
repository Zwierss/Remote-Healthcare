using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorUnsubscribe : ICommand
{
    /// <summary>
    /// "When a client sends a command to unsubscribe, set the client's IsSubscribed property to false."
    /// 
    /// The next function is the one that will be called when a client sends a command to subscribe
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the client.</param>
    /// <param name="Client">The client that sent the command.</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.IsSubscribed = false;
    }
}