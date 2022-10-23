using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class EmergencyStop : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SessionIsActive = false;
        HardwareConnector.StopSessionTimer();
    }
}