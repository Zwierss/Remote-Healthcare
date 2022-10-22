using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorMessage : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        
    }
}