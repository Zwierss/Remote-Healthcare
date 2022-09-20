using Newtonsoft.Json.Linq;

namespace VirtualReality;

public interface Command
{
    void OnCommandReceived(JObject ob, Client client);
}