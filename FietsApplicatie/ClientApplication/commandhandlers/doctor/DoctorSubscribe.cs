using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorSubscribe : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string doctor = packet["data"]!["doctor"]!.ToObject<string>()!;
        if (parent.IsSubscribed)
        {
            parent.Subscribed(doctor, parent.Username, 0);
        }
        else
        {
            parent.Subscribed(doctor, parent.Username, 1);
            parent.Doctor = doctor;
            parent.IsSubscribed = true;
        }

        parent.SendDoctorMessage("Dokter " + doctor + " kijkt nu mee.");
    }
}