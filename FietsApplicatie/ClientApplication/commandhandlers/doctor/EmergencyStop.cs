using FietsDemo;
using Newtonsoft.Json.Linq;
using static ClientApplication.State;

namespace ClientApplication.commandhandlers.doctor;

public class EmergencyStop : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.Callback.OnCallback(Error, "De dokter heeft de applicatie gestopt");
        parent.SendDoctorMessage("Noodstop: De applicatie is gestopt");
        parent.SessionIsActive = false;
        parent.Stop(true);
    }
}