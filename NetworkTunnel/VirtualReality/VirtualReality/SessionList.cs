using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class SessionList : Command
{
    public void OnCommandReceived(JObject ob)
    {
        JObject? currentObject = null;
        DateTime? parsedDate = null;

        foreach (JObject? o in ob["data"]!)
        {
            Console.WriteLine(Environment.UserName);
            Console.WriteLine(Environment.MachineName);

            //try
            //{
                if (o["clientinfo"]["host"].ToObject<string>().ToLower() == Environment.MachineName.ToLower() &&
                    o["clientinfo"]["user"].ToObject<string>().ToLower() == Environment.UserName.ToLower())
                {
                    if (currentObject == null)
                    {
                        currentObject = o;
                        Console.WriteLine(o["lastPing"].ToObject<string>());
                        parsedDate = DateTime.Parse(o["lastPing"].ToObject<string>());
                    }
                    else
                    {
                        if (parsedDate < DateTime.Parse(o["lastPing"].ToObject<string>()))
                        {
                            currentObject = o;
                            parsedDate = DateTime.Parse(o["lastPing"].ToObject<string>());
                        }
                    }
                }
            // }
            // catch
            // {
            //     Console.WriteLine("Werkt niet");
            // }
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