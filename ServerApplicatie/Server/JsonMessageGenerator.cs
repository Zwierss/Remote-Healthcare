using Newtonsoft.Json.Linq;
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
    }
}
