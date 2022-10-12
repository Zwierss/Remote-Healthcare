using Newtonsoft.Json.Linq;

namespace Server;

public interface ICommand
{
    void OnCommandReceived(JObject packet, Client parent);
}