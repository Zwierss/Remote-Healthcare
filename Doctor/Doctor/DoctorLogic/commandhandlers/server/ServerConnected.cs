using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace DoctorLogic.commandhandlers.server;

public class ServerConnected : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        int approved = packet["data"]["status"].ToObject<int>();
        if (approved == 1)
        {
            parent.ViewModel.OnChangedValues();
            parent.SendData(PacketSender.GetJson("server\\getclients.json"));
        }
        else if(approved == 0)
        {
            parent.ViewModel.OnChangedValues("Deze combinatie van gebruikersnaam en wachtwoord bestaat niet.");
        }
    }
}