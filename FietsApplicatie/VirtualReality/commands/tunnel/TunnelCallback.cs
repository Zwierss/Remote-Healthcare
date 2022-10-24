using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.tunnel;

public interface ITunnelCallback
{
    void OnCommandReceived(JObject ob, VRClient parent);
}