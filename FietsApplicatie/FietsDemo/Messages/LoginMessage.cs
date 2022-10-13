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
        
        public LoginMessageData data { get; set; }
    }
    public class LoginMessageData
    {
        public string patientId { get; set; }
    }
}
