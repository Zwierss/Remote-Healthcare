using Newtonsoft.Json.Linq;

namespace VirtualReality
{
    public interface ICommand
    {
        void OnCommandReceived(JObject ob, VRClient vrClient);
    }
}

