using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.client;

public class Subscribed : ICommand
{
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