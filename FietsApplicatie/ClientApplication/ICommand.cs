using Newtonsoft.Json.Linq;

namespace ClientApplication;

public interface ICommand
{
    /// <summary>
    /// > This function is called when a command is received from the client
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the client.</param>
    /// <param name="Client">The client that sent the command</param>
    void OnCommandReceived(JObject packet, Client parent);
}