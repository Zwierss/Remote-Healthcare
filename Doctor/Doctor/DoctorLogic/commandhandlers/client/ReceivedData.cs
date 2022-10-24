using System.Globalization;
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

        string[] args;
        if ((int)speedAvg == -1 || heartrateAvg == -1 || distance == -1 || time == -1)
        {
            args = new[]
            {
                speed.ToString(CultureInfo.InvariantCulture), heartrate.ToString(),
                "--", "--", "--", "--"
            };
        }
        else
        {
            args = new[]
            {
                speed.ToString(CultureInfo.InvariantCulture), heartrate.ToString(),
                speedAvg.ToString(CultureInfo.InvariantCulture), heartrateAvg.ToString(), distance.ToString(),
                time.ToString()
            };
        }
        parent.ViewModel.OnChangedValues(Data, args);
    }
}