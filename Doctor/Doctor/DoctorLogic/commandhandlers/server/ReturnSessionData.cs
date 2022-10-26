using System.Globalization;
using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnSessionData : ICommand
{
    /// <summary>
    /// It takes the values from the packet, converts them to strings, and then sends them to the ViewModel
    /// </summary>
    /// <param name="JObject">The packet received from the server</param>
    /// <param name="DoctorClient">The client that received the command</param>
    public void OnCommandReceived(JObject packet, DoctorClient parent)
    {
        try
        {
            double[] values = packet["data"]!["values"]!.ToObject<double[]>()!;
            string[] args = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                args[i] = values[i].ToString();
            }
            parent.ViewModel.OnChangedValues(Data, args);
        }
        catch (Exception) 
        {
            Console.WriteLine("There were no active values in storage");
        }
    }
}