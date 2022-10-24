using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;

namespace Server.DataSaving
{
    internal class DataSaver
    {

        public static void AddNewClient(Client client)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            string directoryPath = Environment.CurrentDirectory + "\\Clients\\" + client.patientId;
            Directory.CreateDirectory(directoryPath);
            string path = Environment.CurrentDirectory + "\\Clients\\" + client.patientId + "\\" + "InitialPatientFilePatient[" + client.patientId + "].JSON";
            File.Create(path).Close();

            string clientAsJson = JsonConvert.SerializeObject(client);
            File.WriteAllText(path, clientAsJson);
        }

        public static bool ClientExists(string patientId)
        {
            //GetPatientData();
            if (Directory.GetDirectories(Environment.CurrentDirectory + "\\Clients") == null)
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Clients\\PatientZero");
            }


            string[] clientDirectories = Directory.GetDirectories(Environment.CurrentDirectory + "\\Clients");
            foreach (string clientPath in clientDirectories)
            {
                string path = Environment.CurrentDirectory + "\\Clients\\" + patientId;
                if (clientPath == path)
                {
                    return true;
                }
            }
            return false;
        }
        
        public static void AddPatientFile(string patientId)
        {
            //JObject jObject = Client.ReadMessage(client);


            int amountOfFiles = Directory.GetFiles(Environment.CurrentDirectory + "\\Clients\\" + patientId).Length;
            string path = Environment.CurrentDirectory + "\\Clients\\" + patientId + "\\[" + patientId + "] session#" + amountOfFiles +
                          ".JSON";

            File.Create(path).Close();
            File.WriteAllText(path, "data");
        }

        //get session data
        public static string GetPatientSession(string patientId, int session)
        {
            //string file = Environment.CurrentDirectory + "\\Clients\\[" + patientId + "] session#" + session + ".JSON";
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Clients\\" + patientId);


            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                //idk man
                if (session.Equals(int.Parse(Regex.Replace(fileName.Substring(patientId.Length + 1), "[^0-9]", ""))))
                {
                    Console.WriteLine(file);
                    return file;
                }
            }
            return null;

        }

        //get list of session
        public static List<Tuple<int, string>> GetPatientSessions(string patientId)
        {
            Console.WriteLine("a");
            var sessions = new List<Tuple<int, string>>();
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Clients\\" + patientId);
            foreach (string file in files)
            {
                if (file.Contains("session#"))
                {
                    string fileName = Path.GetFileName(file);
                    int sessionCount = int.Parse(Regex.Replace(fileName.Substring(patientId.Length+1), "[^0-9]", ""));
                    sessions.Add(new Tuple<int, string>(sessionCount, ""));
                }
            }
           
            return sessions;
        }

        //get patientids
        public static string[] GetPatientIds()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            string[] files = Directory.GetDirectories(Environment.CurrentDirectory + "\\Clients");
            foreach (string file in files)
            Console.WriteLine(Path.GetFileName(file));
            Console.WriteLine("done + \n");
            return files;
        }
    }
}