using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class StartSession : ICommand
{
    /// <summary>
    /// When a client sends a command to the server, the server will start a timer that will close the session if the client
    /// doesn't send another command within a certain amount of time
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the client.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SessionIsActive = true;
        HardwareConnector.StartSessionTimer();
    }
}