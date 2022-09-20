using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualReality.commands
{
    public class TunnelSend : Command
    {
        public void OnCommandReceived(JObject ob, Client client)
        {
            Console.WriteLine("Json: " + ob["data"]["data"]["status"]);
            
        }
    }
}
