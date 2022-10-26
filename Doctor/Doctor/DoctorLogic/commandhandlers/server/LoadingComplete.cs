using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoctorLogic.State;

namespace DoctorLogic.commandhandlers.server
{
    /* It's a command that is sent from the server to the client when the client has successfully loaded the game */
    public class LoadingComplete : ICommand
    {
        public void OnCommandReceived(JObject packet, DoctorClient parent)
        {
            parent.ViewModel.OnChangedValues(Success);
        }
    }
}
