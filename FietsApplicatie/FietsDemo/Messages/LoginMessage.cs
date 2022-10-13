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
        
        public LoginData data { get; set; }
    }
    public class LoginData
    {
        public string patientId { get; set; }
    }
}
