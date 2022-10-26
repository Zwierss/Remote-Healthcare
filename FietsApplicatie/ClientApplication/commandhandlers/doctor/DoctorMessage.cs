using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorMessage : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string data = packet["data"]!["data"]!["message"]!.ToObject<string>()!;
        parent.SendDoctorMessage(packet["data"]!["data"]!["sender"]!.ToObject<string>()! + ": " + data);
    }
}