using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualReality.commands
{
    public class ChangeTime : Command
    {
        public void OnCommandReceived(JObject ob, Client client)
        {
            Console.WriteLine("TimeJson: " + ob.ToString());
            //client.SetTime(ob["data"]["id"].ToObject<string>());
            /*
            if (ob["data"]["status"].ToObject<string>().Equals("ok"))
            {
                string message = ob["data"]["id"].ToObject<string>();
                client.SetTime(message);
            }
            */
            Console.WriteLine("Done with command");

            //Client.GetInstance().StartReading((JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText("JSON/get.json"))));
        }
    }
}
