using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class SetResistance : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        Console.WriteLine("works");
        HardwareConnector.SetResistance(packet["data"]!["data"]!["resistance"]!.ToObject<byte>());
        parent.SendDoctorMessage("Weerstand: is veranderd naar " + packet["data"]!["data"]!["resistance"]!.ToObject<byte>());
    }
}