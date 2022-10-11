using FietsDemo.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FietsDemo
{
    public class DataMessage
    {
        public string id { get; set; }
        public SpecificDataMessage data { get; set; }  
    }
    public class SpecificDataMessage
    {
        public int heartrate { get; set; }
        public double speed{ get; set; }
        public DateTime time { get; set; }
        public int timestamp { get; set; }
        public bool endOfSession { get; set; }
    }
    
}