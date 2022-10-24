using Newtonsoft.Json.Linq;

namespace ClientApplication;

public interface ICommand
{
    void OnCommandReceived(JObject packet, Client parent);
}