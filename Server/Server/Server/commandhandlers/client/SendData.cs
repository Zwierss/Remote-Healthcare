using Newtonsoft.Json.Linq;

namespace Server.commandhandlers.client;

public class SendData : ICommand
{
    /// <summary>
    /// It stores the session data in the database and then passes the packet to the next function
    /// </summary>
    /// <param name="JObject">The packet received from the client.</param>
    /// <param name="Client">The client that sent the packet</param>
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