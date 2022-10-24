using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.client;

public class ReceivedData : ICommand
{
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        double speed = packet["data"]!["data"]!["speed"]!.ToObject<double>();
        int heartrate = packet["data"]!["data"]!["heartrate"]!.ToObject<int>();
        double speedAvg = packet["data"]!["data"]!["speedavg"]!.ToObject<double>();
        int heartrateAvg = packet["data"]!["data"]!["avgheartrate"]!.ToObject<int>();
        int distance = packet["data"]!["data"]!["distance"]!.ToObject<int>();
        int time = packet["data"]!["data"]!["time"]!.ToObject<int>(); 
        
        string arg = speed + "+" + heartrate + "+" + speedAvg + "+" + heartrateAvg + "+" + distance + "+" + time;
        parent.ViewModel.OnChangedValues(Data, arg);
    }
}