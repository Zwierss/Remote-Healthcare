using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class Program
{
    public static void Main(string[] args)
    {
#pragma warning disable CS4014
        Client.GetInstance().StartConnection();
#pragma warning restore CS4014

        int count = 0;
        bool onlyOnce = true;
        
        while (true)
        {
            // if (count == 100)
            // {
            //     count = 0;
            //     Client.GetInstance().SendData((JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText("JSON/get.json"))));
            // }
            // count++;
            
            Thread.Sleep(10);
        }
    }
}