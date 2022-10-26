using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorSubscribe : ICommand
{
    /// <summary>
    /// It checks if the client is subscribed to a doctor, if so, it unsubscribes, if not, it subscribes
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command.</param>
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