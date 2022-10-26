using Newtonsoft.Json.Linq;

namespace VirtualReality
{
    /* An interface that is used to define the method that will be called when a command is received. */
    public interface ICommand
    {
        void OnCommandReceived(JObject ob, VRClient vrClient);
    }
}

