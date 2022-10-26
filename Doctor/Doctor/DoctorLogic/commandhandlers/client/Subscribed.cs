using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.client;

public class Subscribed : ICommand
{
    /// <summary>
    /// It checks if the status is 0, if so, it will show an error message, if not, it will show a success message
    /// </summary>
    /// <param name="JObject">The packet that was received from the server</param>
    /// <param name="DoctorClient">The client that received the command</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        int status = packet["data"]!["status"]!.ToObject<int>();
        if (status == 0)
        {
            parent.ViewModel.OnChangedValues(Error, new[]{"Er is al een andere dokter aan eze client verbonden"});
        }
        else if(status == 1)
        {
            parent.ViewModel.OnChangedValues(Success);
        }
    }
}