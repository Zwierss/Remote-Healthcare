using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorMessage : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string data = packet["data"]!["data"]!["message"]!.ToObject<string>()!;
        parent.SendDoctorMessage(data);
        Thread.Sleep(8000);
        parent.SendDoctorMessage("");
    }
}