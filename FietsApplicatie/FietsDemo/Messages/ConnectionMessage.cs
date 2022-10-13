using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FietsDemo
{
    public class ConnectionMessage
    {
        public string id { get; set; }
        public ConnectionMessageData data { get; set; }
    }
    public class ConnectionMessageData
    {
        public bool bike { get; set; }
        public bool heartrate { get; set; }
    }

}