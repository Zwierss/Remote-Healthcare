using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FietsDemo.JSON
{
    public class LoginMessage
    {
        public string id { get; set; }
        
        public SpecificLoginMessage data { get; set; }
    }
    public class SpecificLoginMessage
    {
        public string patientId { get; set; }
    }
}
