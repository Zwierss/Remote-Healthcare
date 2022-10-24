using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class StartSession : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.Doctor = packet["data"]!["doctor"]!.ToObject<string>()!;
        parent.SessionIsActive = true;
        HardwareConnector.StartSessionTimer();
    }
}