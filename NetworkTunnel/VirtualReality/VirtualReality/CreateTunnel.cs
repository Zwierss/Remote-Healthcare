using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class CreateTunnel : Command
{
    public void OnCommandReceived(JObject ob)
    {
        if(ob["data"]["status"].ToObject<string>().Equals("ok"))
        {
            Client.GetInstance().SetTunnel(ob["data"]["id"].ToObject<string>());
            //JObject o = new JObject("JSON/resetscene.json");
            //Client.GetInstance().SendData(o);
        }
    }
}