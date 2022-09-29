using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FietsDemo
{
    public class JsonMessage
    {
        public string id { get; set; }
        public JsonData Data { get; set; }  
    }
    public class JsonData
    {
        public int heartrate { get; set; }
        public double speed{ get; set; }
        public DateTime time { get; set; }
        public int timestamp { get; set; }
        public bool endOfSession { get; set; }

    }
    
}