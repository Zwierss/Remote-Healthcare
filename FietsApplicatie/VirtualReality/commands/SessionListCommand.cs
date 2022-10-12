using System.Globalization;
using Newtonsoft.Json.Linq;

namespace VirtualReality.commands;

public class SessionListCommand : ICommand
{
    private const string Format = "MM/dd/yyyy HH:mm:ss";

    public void OnCommandReceived(JObject ob, VRClient vrClient)
    {
        JObject? currentObject = null;
        DateTime? parsedDate = null;

        foreach (var jToken in ob["data"]!)
        {
            var o = (JObject?)jToken;
            Console.WriteLine(Environment.UserName);
            Console.WriteLine(Environment.MachineName);

            if (!string.Equals(o!["clientinfo"]!["host"]!.ToObject<string>(), Environment.MachineName,
                    StringComparison.CurrentCultureIgnoreCase) ||
                !string.Equals(o["clientinfo"]!["user"]!.ToObject<string>()!, Environment.UserName,
                    StringComparison.CurrentCultureIgnoreCase)) continue;
            if (currentObject == null)
            {
                currentObject = o;
                Console.WriteLine(o["lastPing"]!.ToObject<string>());
                string dateInString = o["lastPing"]!.ToObject<string>()!;
                parsedDate = DateTime.ParseExact(dateInString!, Format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            else
            {
                if (!(parsedDate < DateTime.ParseExact(o["lastPing"]!.ToObject<string>()!, Format,
                        CultureInfo.InvariantCulture, DateTimeStyles.None))) continue;
                currentObject = o;
                parsedDate = DateTime.ParseExact(o["lastPing"]!.ToObject<string>()!, Format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
        }
        
        if (currentObject != null)
        {
            vrClient.CreateTunnel(currentObject["id"]!.ToObject<string>()!);
        }
        else
        {
            Console.WriteLine("Could not find user...");
        }
    }
}