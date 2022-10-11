using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class JsonMessageGenerator
    {
        public static JObject GetJsonOkMessage(string id)
        {
            JObject message = new JObject();
            message.Add("id", id);
            message.Add("status", "ok");
            
            Console.WriteLine(message.ToString());
            return message;
        }

        public static JObject GetJsonLoggedInMessage(bool newAccount)
        {
            JObject message = new JObject();
            message.Add("id", "client/login");
            message.Add("newAccount", newAccount);

            Console.WriteLine(message.ToString());
            return message;
        }
    }
}