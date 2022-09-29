using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApplication
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

        public static string GetJsonLoginMessage(string username, string password)
        {
            JObject message = new JObject();
            message.Add("id", "server/login");
            message.Add("username", username);
            message.Add("password", password);

            Console.WriteLine(message.ToString());
            return message.ToString();
        }
    }
}
