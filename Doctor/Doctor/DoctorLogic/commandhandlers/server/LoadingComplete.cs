using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server
{
    public class LoadingComplete : ICommand
    {
        public void OnCommandReceived(JObject packet, DoctorClient parent)
        {
            parent.ViewModel.OnChangedValues(Success);
        }
    }
}
