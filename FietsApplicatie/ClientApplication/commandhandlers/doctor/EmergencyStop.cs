using FietsDemo;
using Newtonsoft.Json.Linq;
using static ClientApplication.State;

namespace ClientApplication.commandhandlers.doctor;

public class EmergencyStop : ICommand
{
    /// <summary>
    /// The doctor has stopped the application
    /// </summary>
    /// <param name="JObject">The packet that was received from the client.</param>
    /// <param name="Client">The client that received the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.Callback.OnCallback(Error, "De dokter heeft de applicatie gestopt");
        parent.SendDoctorMessage("Noodstop: De applicatie is gestopt");
        parent.SessionIsActive = false;
        parent.Stop(true);
    }
}