using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class StopSession : ICommand
{
    /// <summary>
    /// This function is called when the client sends a command to stop the session
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the client.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SessionIsActive = false;
        parent.CollectedSpeeds.Clear();
        parent.CollectedRates.Clear();
        parent.PrevDistance = 0;
        HardwareConnector.StopSessionTimer();
    }
}