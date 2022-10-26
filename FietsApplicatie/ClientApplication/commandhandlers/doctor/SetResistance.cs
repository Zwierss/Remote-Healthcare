using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class SetResistance : ICommand
{
    /// <summary>
    /// It sets the resistance of the hardware connector to the resistance that was sent by the client
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Console.WriteLine("works");
        HardwareConnector.SetResistance(packet["data"]!["data"]!["resistance"]!.ToObject<byte>());
        parent.SendDoctorMessage("Weerstand: is veranderd naar " + packet["data"]!["data"]!["resistance"]!.ToObject<byte>());
    }
}