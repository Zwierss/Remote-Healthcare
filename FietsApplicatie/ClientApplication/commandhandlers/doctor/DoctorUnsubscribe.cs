using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorUnsubscribe : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        parent.IsSubscribed = false;
    }
}