using System.Globalization;
using Newtonsoft.Json.Linq;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server;

public class ReturnSessionData : ICommand
{
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