using Newtonsoft.Json.Linq;

namespace DoctorLogic;

public interface ICommand
{
    void OnCommandReceived(JObject packet, DoctorClient parent);
}