using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.client;

public class SendData : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        if (parent.SessionActive)
        {
            JObject o = (JObject)packet["data"]!["data"]!;
            double[] objects =
            {
                o["speed"]!.ToObject<double>(),
                o["heartrate"]!.ToObject<double>(),
                o["speedavg"]!.ToObject<double>(),
                o["avgheartrate"]!.ToObject<double>(),
                o["distance"]!.ToObject<double>(),
                o["time"]!.ToObject<double>(),
            };
            parent.StorageManager.StoreSession(objects, parent.CurrentSessionTitle);
        }

        Console.WriteLine(packet);

        new Switch().OnCommandReceived(packet, parent);
    }
}