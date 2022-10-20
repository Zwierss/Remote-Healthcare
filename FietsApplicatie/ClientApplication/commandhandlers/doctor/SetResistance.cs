using FietsDemo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.commandhandlers.doctor
{
    public class SetResistance : ICommand
    {
        public void OnCommandReceived(JObject packet, Client parent)
        {
            try
            {
                HardwareConnector.SetResistance(packet["resistance"]!.ToObject<byte>());
                Console.WriteLine("resistance changed");
            }
            catch
            {
                Console.WriteLine("No such thing found as resistance");
            }
        }
    }
}
