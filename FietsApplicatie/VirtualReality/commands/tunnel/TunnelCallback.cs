using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.tunnel;

public interface ITunnelCallback
{
    /// <summary>
    /// > This function is called when a command is received from the server
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the server.</param>
    /// <param name="VRClient">The VRClient that received the command.</param>
    void OnCommandReceived(JObject ob, VRClient parent);
}