using FietsDemo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.commandhandlers.doctor
{
    public class Resistance : ICommand
    {
        public void OnCommandReceived(JObject packet, Client parent)
        {

        }
    }
}
