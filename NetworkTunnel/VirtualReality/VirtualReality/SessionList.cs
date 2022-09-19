using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class SessionList : Command
{

    public static string _FORMAT = "dd/MM/yyyy HH:mm:ss";
    
    public void OnCommandReceived(JObject ob)
    {
        JObject? currentObject = null;
        DateTime? parsedDate = null;

        foreach (JObject? o in ob["data"]!)
        {
            Console.WriteLine(Environment.UserName);
            Console.WriteLine(Environment.MachineName);

            try
            {
                if (o["clientinfo"]["host"].ToObject<string>().ToLower() == Environment.MachineName.ToLower() &&
                    o["clientinfo"]["user"].ToObject<string>().ToLower() == Environment.UserName.ToLower())
                {
                    
                    if (currentObject == null)
                    {
                        currentObject = o;
                        Console.WriteLine(o["lastPing"].ToObject<string>());
                        parsedDate = DateTime.ParseExact(o["lastPing"].ToObject<string>(), _FORMAT, null);
                    }
                    else
                    {
                        if (parsedDate < DateTime.ParseExact(o["lastPing"].ToObject<string>(), _FORMAT, null))
                        {
                            currentObject = o;
                            parsedDate = DateTime.ParseExact(o["lastPing"].ToObject<string>(), _FORMAT, null);
                        }
                    }
                }
            }
            catch
            { Console.WriteLine("Werkt niet"); }
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