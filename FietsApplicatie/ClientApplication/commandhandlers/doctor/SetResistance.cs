using FietsDemo;
using Newtonsoft.Json.Linq;

namespace ClientApplication.commandhandlers.doctor;

public class SetResistance : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        HardwareConnector.SetResistance(packet["data"]!["data"]!["resistance"]!.ToObject<byte>());
    }
}