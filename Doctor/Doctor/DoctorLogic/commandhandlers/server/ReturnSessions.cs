using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnSessions : ICommand
{
    /// <summary>
    /// It takes the data from the packet, and sends it to the ViewModel to be displayed
    /// </summary>
    /// <param name="JObject">The packet that was received from the client.</param>
    /// <param name="DoctorClient">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        try
        {
            string[] args = packet["data"]!["files"]!.ToObject<string[]>()!;
            Console.WriteLine(args[0]);
            parent.ViewModel.OnChangedValues(Data, args);
        }
        catch (Exception) 
        {
            Console.WriteLine("There were no active values in storage");
        }
    }
}