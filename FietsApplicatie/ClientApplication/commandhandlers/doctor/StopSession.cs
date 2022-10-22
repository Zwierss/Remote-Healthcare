using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class StopSession : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.SessionIsActive = false;
        HardwareConnector.StopSessionTimer();
    }
}