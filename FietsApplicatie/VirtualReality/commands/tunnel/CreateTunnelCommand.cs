﻿using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.tunnel;

public class CreateTunnelCommand : ICommand
{
    public void OnCommandReceived(JObject ob, VRClient vrClient)
    {
        if(ob["data"]!["status"]!.ToObject<string>()!.Equals("ok"))
        {
            vrClient.SetTunnel(ob["data"]!["id"]!.ToObject<string>()!);
        }
        Console.WriteLine("Done with command");
        
        //Client.GetInstance().StartReading((JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText("JSON/get.json"))));
    }
}