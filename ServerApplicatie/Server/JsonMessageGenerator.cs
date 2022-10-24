using Newtonsoft.Json.Linq;
using Server.DataSaving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class JsonMessageGenerator
    {
        public static string GetJsonOkMessage(string id)
        {
            JObject message = new JObject();
            message.Add("id", id);
            message.Add("status", "ok");

            Console.WriteLine(message.ToString());
            return message.ToString();
        }

        public static string GetJsonLoggedinMessage(bool newAccount)
        {
            JObject message = new JObject();
            message.Add("id", "server/login");
            message.Add("newAccount", newAccount);

            Console.WriteLine(message.ToString());
            return message.ToString();
        }

        public static string GetJsonStartSessionMessage(string patiendId)
        {
            JObject message = new JObject();
            message.Add("id", "server/startSession");
            message.Add("client", patiendId);

            return message.ToString();
        }
        
        public static string GetJsonStopSessionMessage(string patiendId)
        {
            JObject message = new JObject();
            message.Add("id", "server/stopSession");
            message.Add("client", patiendId);

            return message.ToString();
        }

        public static string GetJsonSessionDataMessage(int heartRate, int speed, string time, string timestamp, bool endOfSession)
        {
            JObject message = new JObject();
            message.Add("id", "server/received");

            JObject messageData = new JObject();
            messageData.Add("heartrate", heartRate);
            messageData.Add("speed", speed);
            messageData.Add("time", time);
            messageData.Add("timestamp", timestamp);
            messageData.Add("endOfSession", endOfSession);

            message.Add("data", messageData);

            return message.ToString();
        }
    }

    public class GetJsonPatientIdsMessage
    {
        public string id = "server/patientIds";
        public string[] data { get; set; }
    }

    public class GetJsonSessionsMessage
    {
        public string id = "server/sessionList";
        public List<Tuple<int, string>> data { get; set; }

    }

    public class GetJsonSessionDataMessage
    {
        public string id = "server/sessionData";
        public string[] data { get; set; }
    }
}
