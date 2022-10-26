using Newtonsoft.Json.Linq;

namespace DoctorLogic;

public interface ICommand
{
    /// <summary>
    /// > This function is called when a command is received from the server
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the client.</param>
    /// <param name="DoctorClient">The client that sent the command.</param>
    void OnCommandReceived(JObject packet, DoctorClient parent);
}