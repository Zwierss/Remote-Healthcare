using System.Globalization;
using Newtonsoft.Json.Linq;

namespace VirtualReality.commands;

public class SessionListCommand : ICommand
{
    private const string Format = "MM/dd/yyyy HH:mm:ss";

    /// <summary>
    /// It takes the JSON object that was received from the server, and then it loops through the data array. It then checks
    /// if the host and user name of the current object is the same as the host and user name of the current machine. If it
    /// is, it checks if the current object is the first object that was found, and if it is, it sets the current object to
    /// the current object and sets the parsed date to the last ping of the current object. If it isn't the first object, it
    /// checks if the parsed date is less than the last ping of the current object, and if it is, it sets the current object
    /// to the current object and sets the parsed date to the last ping of the current object. After the loop is done, it
    /// checks if the current object is null, and if it isn't, it creates a tunnel to the current object. If it is, it
    /// prints out that it couldn't find the user
    /// </summary>
    /// <param name="JObject">The JObject that was received from the server.</param>
    /// <param name="VRClient">The VRClient object that is used to create the tunnel.</param>
    public void OnCommandReceived(JObject ob, VRClient vrClient)
    {
        JObject? currentObject = null;
        DateTime? parsedDate = null;

        foreach (var jToken in ob["data"]!)
        {
            var o = (JObject?)jToken;

            if (!string.Equals(o!["clientinfo"]!["host"]!.ToObject<string>(), Environment.MachineName,
                    StringComparison.CurrentCultureIgnoreCase) ||
                !string.Equals(o["clientinfo"]!["user"]!.ToObject<string>()!, Environment.UserName,
                    StringComparison.CurrentCultureIgnoreCase)) continue;
            if (currentObject == null)
            {
                currentObject = o;
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