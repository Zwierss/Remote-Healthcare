using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server.DataSaving
{
    internal class DataSaver
    {

        public static void AddNewClient(Client client)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            string directoryPath = Environment.CurrentDirectory + "\\Clients\\" + client.PatientId;
            Directory.CreateDirectory(directoryPath);
            string path = Environment.CurrentDirectory + "\\Clients\\" + client.PatientId + "\\" + client.PatientId + ".JSON";
            File.Create(path).Close();

            // ClientData clientData = new ClientData()
            // {
            //     patentId = client.patientId
            // };
            
            string clientAsJson = JsonConvert.SerializeObject(client);
            File.WriteAllText(path, clientAsJson);
        }

        public static bool ClientExists(string patientId)
        {
            string[] clientFiles = Directory.GetFiles(Environment.CurrentDirectory + "\\Clients");
            foreach (string clientPath in clientFiles)
            {
                // var clientInJson = JObject.Parse(File.ReadAllText(clientPath));
                // Client client = new Client();
                // client.patientId = clientInJson["patientId"].ToString();
                // client.patientId = clientPath;
                if(clientPath == patientId)
                {
                    return true;
                }
            }

            return false;
        }

        public static void AddPatientFile(TcpClient client, List<JObject> sessionData)
        {
            JObject jObject = JObject.Parse(Client.ReadJsonMessage(client));
            String patientId = jObject["data"]["patientId"].ToString();

            int amountOfFiles = Directory.GetFiles(Environment.CurrentDirectory + "\\Clients\\" + patientId).Length;
            
            string path = Environment.CurrentDirectory + "\\Clients\\" + patientId + "\\" + patientId + " session#" + amountOfFiles +
                          ".JSON";
            File.Create(path).Close();

            File.WriteAllText(path, sessionData.ToString());
        }
    }
}