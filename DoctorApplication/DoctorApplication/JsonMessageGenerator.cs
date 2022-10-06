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

        public static string GetJsonClientSelectMessage(List<string> patientIds)
        {
            JObject message = new JObject();
            message.Add("id", "server/clients");
            message.Add("amountOfClients", patientIds.Count);
            JObject clients = new JObject();
            for(int i = 0; i < patientIds.Count; i++)
            {
                clients.Add("client" + i, patientIds[i]);
            }
            message.Add("data", clients);

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
        
        public static string GetJsonSendMessage(string patiendId, string message)
        {
            JObject messageToSend = new JObject();
            messageToSend.Add("id", "server/sent");
            messageToSend.Add("client", patiendId);
            messageToSend.Add("message", message);

            return messageToSend.ToString();
        }
        
        public static string GetJsonEmergencyStopMessage(string patiendId)
        {
            JObject message = new JObject();
            message.Add("id", "server/emergencyStop");
            message.Add("client", patiendId);

            return message.ToString();
        }
    }
}
