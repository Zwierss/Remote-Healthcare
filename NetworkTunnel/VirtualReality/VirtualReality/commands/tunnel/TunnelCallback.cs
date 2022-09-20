using Newtonsoft.Json.Linq;

namespace VirtualReality;

public interface TunnelCallback
{
    void OnCommandReceived(JObject ob, TunnelCommand parent);
}