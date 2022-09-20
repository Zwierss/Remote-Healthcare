using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class CreateTunnel : Command
{
    public void OnCommandReceived(JObject ob, Client client)
    {
        if(ob["data"]["status"].ToObject<string>().Equals("ok"))
        {
            client.SetTunnel(ob["data"]["id"].ToObject<string>());
        }
        Console.WriteLine("Done with command");
        
        //Client.GetInstance().StartReading((JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText("JSON/get.json"))));
    }
}