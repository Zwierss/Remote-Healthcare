using Newtonsoft.Json.Linq;

namespace VirtualReality;

public interface Command
{
    public void OnCommandReceived(JObject ob);
}