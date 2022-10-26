using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.tunnel;

public class CreateTunnelCommand : ICommand
{
    /// <summary>
    /// > This function is called when a command is received from the server
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the server.</param>
    /// <param name="VRClient">The client that sent the command.</param>
    public void OnCommandReceived(JObject ob, VRClient vrClient)
    {
        if(ob["data"]!["status"]!.ToObject<string>()!.Equals("ok"))
        {
            vrClient.SetTunnel(ob["data"]!["id"]!.ToObject<string>()!);
        }
        
    }
}