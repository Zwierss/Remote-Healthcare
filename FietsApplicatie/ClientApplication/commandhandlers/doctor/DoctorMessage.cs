using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class DoctorMessage : ICommand
{
    /// <summary>
    /// It takes a packet, and sends a message to the doctor
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string data = packet["data"]!["data"]!["message"]!.ToObject<string>()!;
        parent.SendDoctorMessage(packet["data"]!["data"]!["sender"]!.ToObject<string>()! + ": " + data);
    }
}