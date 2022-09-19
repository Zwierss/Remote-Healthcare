using System.Globalization;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class SessionList : Command
{

    public static string _FORMAT = "MM/dd/yyyy HH:mm:ss";
    
    public void OnCommandReceived(JObject ob)
    {
        JObject? currentObject = null;
        DateTime? parsedDate = null;

        foreach (JObject? o in ob["data"]!)
        {
            Console.WriteLine(Environment.UserName);
            Console.WriteLine(Environment.MachineName);

            // try
            // {
                if (o["clientinfo"]["host"].ToObject<string>().ToLower() == Environment.MachineName.ToLower() &&
                    o["clientinfo"]["user"].ToObject<string>().ToLower() == Environment.UserName.ToLower())
                {
                    
                    if (currentObject == null)
                    {
                        currentObject = o;
                        Console.WriteLine(o["lastPing"].ToObject<string>());
                        string dateInString = o["lastPing"].ToObject<string>();
                        parsedDate = DateTime.ParseExact(dateInString, _FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                    else
                    {
                        if (parsedDate < DateTime.ParseExact(o["lastPing"].ToObject<string>(), _FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None))
                        {
                            currentObject = o;
                            parsedDate = DateTime.ParseExact(o["lastPing"].ToObject<string>(), _FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None);
                        }
                    }
                }
            // }
            // catch
            // { Console.WriteLine("Werkt niet"); }
        }
        
        if (currentObject != null)
        {
            Client.GetInstance().CreateTunnel(currentObject["id"].ToObject<string>());
        }
        else
        {
            Console.WriteLine("Could not find user...");
        }
    }
}