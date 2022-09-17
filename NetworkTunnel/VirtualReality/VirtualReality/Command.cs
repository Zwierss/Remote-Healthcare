using Newtonsoft.Json.Linq;

namespace VirtualReality;

public interface CommandHandler
{
    public void handleCommand(Client client, JObject ob);
}