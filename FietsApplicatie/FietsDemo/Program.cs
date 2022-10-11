using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Avans.TI.BLE;
using FietsDemo.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FietsDemo;

public static class Program
{
    public static void Main(string[] args)
    {
        private static TcpClient _client;
        private static NetworkStream _stream;
        private static string _username;
        private static bool _stop = false;
        private static bool _start = false;
        private static bool _first = true;
        private static bool _emergency = false;
        private static string _host = "localhost";
        //192.168.43.50
        private static int _port = 15243;
        private static bool _connected = false;
        private static int _heartRate = 0;
        private static int _elapsedTime = 0;
        private static double _distanceTraveled = 0;
        private static int _startElapsedTime = 0;
        private static int _previousElapsedTime;

        public static Task Main(string[] args)
        {

            //runSimulation();

            Console.WriteLine("Client started");
            _client = new TcpClient();
            _client.BeginConnect(_host, _port, new AsyncCallback(OnConnect), null);

            new Thread(() =>
            {
                while (true)
                {
                    ReadJsonMessage(_client);
                }

            }).Start();

            Console.WriteLine("Typ uw pati�ntnummer");
            _username = Console.ReadLine();
            Console.WriteLine("patientId sent");
            SendPatientId();

            new Thread(() =>
            {
                while (true)
                {
                    switch (Console.ReadLine())
                    {
                        case "stop":
                            _stop = true;
                            break;

                        case "start":
                            _stop = false;
                            _start = true;
                            _first = true;
                            break;
                    }
                }

            }).Start();

            new Thread(() =>
            {
                while (true)
                {
                    ReadJsonMessage(_client);
                }

            }).Start();

            //_username = Console.ReadLine();


            Bike bike = new Bike();
            HeartRate heart = new HeartRate();
            
            Console.WriteLine("Trying connection with devices");
            //bool bikeConnection = bike.MakeConnection().Result;
            bool bikeConnection = false;
            //Thread.Sleep(10000);


            bool hearRateConnection = heart.MakeConnection().Result;
            //Thread.Sleep(10000);

            //bool clientConnection = _client.MakeConnection().Result;


            if (!bikeConnection)
            {
                if (!hearRateConnection)
                {
                    //Console.WriteLine("Could not connect with the devices. DO you want to connect with the simulator? (y/n)");
                    //string input = Console.ReadLine();
                    //if (input == "y")
                    //{
                    Console.WriteLine("Starting Simulation");
                    while (true)
                    {
                        runSimulation();
                        Simulator.Reset();
                    }
                    
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Closing down application");
                    //    return Task.CompletedTask;
                    //}
                }
            }
            //while (true)
            //{
            //    if (!_start)
            //    {
            //        bike.Reset();
            //        heart.Reset();
            //    }
            //}
            return Task.CompletedTask;
        }

        public static void runSimulation()
        {
            //bool running = true;
            while (_start)
            {
                Thread.Sleep(250);
                int[] values = Simulator.SimulateGeneralData();
                PrintGeneralData(values);
                Thread.Sleep(250);
                int[] bikeData = Simulator.SimulateBikeData();
                PrintBikeData(bikeData);
                ConvertToJson(values);
            }

        }


        public static void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            string[] data = BitConverter.ToString(e.Data).Split('-');
            int[] values = new int[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                values[i] = int.Parse(data[i], System.Globalization.NumberStyles.HexNumber);
            }

            if (data.Length < 13)
            {
                PrintHeartData(values);
            }
            else
            {
                switch (data[4])
                {
                    case "10":
                        if(_first)
                        {
                            _first = false;
                            _startElapsedTime = values[6];
                            _previousElapsedTime = values[6];
                            Console.WriteLine(_startElapsedTime);
                        }
                        Console.WriteLine((_previousElapsedTime) + " " + values[6] + " " + (values[9] + (values[8] << 8)) + " " + (_previousElapsedTime - _startElapsedTime));

                        if (_previousElapsedTime != values[6])
                        {
                            _distanceTraveled += (values[9] + (values[8] << 8) * 0.001) / 4;
                            _previousElapsedTime+=2;
                            values[6] = _previousElapsedTime - _startElapsedTime;


                            PrintGeneralData(values);
                            ConvertToJson(values);
                        }
                        break;
                    case "19":
                        PrintBikeData(values);
                        //convertData(values);
                        break;
                }
            }



        }

        private static void PrintGeneralData(int[] values)
        {
            if(_start)
            {
                Console.WriteLine("Received General Data");
                Console.WriteLine("-----------");
                Console.WriteLine("Equipment Type: " + values[5]);
                Console.WriteLine("Elapsed Time: " + (values[6]) + " seconds");
                Console.WriteLine("Distance Traveled: " + values[7] + " meters");
                Console.WriteLine("Speed: " + (values[9] + (values[8] << 8) * 0.001) + " m/s");
                Console.WriteLine("Heart Rate: " + _heartRate + " bpm");
                Console.WriteLine("-----------");
            }
           

        }

        private static void PrintBikeData(int[] values)
        {
            if(_start)
            {
                Console.WriteLine("Received Bike Data");    
                Console.WriteLine("-----------");
                Console.WriteLine("Event Count: " + values[5]);
                Console.WriteLine("Instantaneous Cadence: " + values[6] + " rpm");
                Console.WriteLine("Accumulated Power: " + (values[7] + (values[8] << 8)) + " W");
                string splitted = Convert.ToString(values[10], 2);
                Console.WriteLine("Instantaneous Power: " + (values[9] + Convert.ToInt32(splitted.Substring(0, 4), 2) << 8) + " W");
                Console.WriteLine("Trainer Status: " + Convert.ToInt32(splitted.Substring(3, 4), 2));
                Console.WriteLine("-----------");
            }
            
        }

        public static void BleHeartRate_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            string[] data = BitConverter.ToString(e.Data).Split('-');
            int[] values = new int[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                values[i] = int.Parse(data[i], System.Globalization.NumberStyles.HexNumber);
            }

            PrintHeartData(values);
        }

        private static void PrintHeartData(int[] values)
        {
            if(_start) { 
                _heartRate = values[1];
                Console.WriteLine("Received Heart Rate Data");
                Console.WriteLine("-----------");
                Console.WriteLine(values[1] + " bpm");
                Console.WriteLine("-----------");
                
            }
           
        }

        private static void OnConnect(IAsyncResult ar)
        {
            try
            {
                _client.EndConnect(ar);
                _connected = true;
                Console.WriteLine("Verbonden!");
            }
            catch
            {
                Console.WriteLine("Kan geen verbinding maken met server");
            }


        }
        private static void ConvertToJson(int[] values)
        {
            //JObject jsonString = new JObject();
            //JObject dataString = new JObject();
            //dataString.Add("heartrate", values[10]);
            //dataString.Add("speed", values[9] + (values[8] << 8) * 0.001);
            //dataString.Add("time", "");
            //dataString.Add("timestamp", values[6]);
            //dataString.Add("endOfSession", false);
            //jsonString.Add("id", "client/received");
            //jsonString.Add("data", dataString
            if (_start)
            {
                DataMessage dataMessage = new DataMessage()
                {
                    id = "server/received",
                    data = new SpecificDataMessage()
                    {
                        heartrate = values[10],
                        speed = (values[9] + (values[8] << 8)) * 0.001,
                        time = DateTime.Now,
                        timestamp = values[6],
                        endOfSession = _stop || _emergency
                    }
                };

                SendData(JsonConvert.SerializeObject(dataMessage));
            }

            //SendData(PacketSender.SendReplacedObject("session", id, 1, "createtunnel.json"));
        }

        //Sends messages to server
        private static void SendData(string ob)
        {
            var stream = new StreamWriter(_client.GetStream(), Encoding.ASCII);
            {
                stream.Write(ob + "\n");
                stream.Flush();
                Console.WriteLine("sent!");
                if (_emergency)
                {
                    Console.WriteLine("stopped");
                    _client.Close();
                }
                if (_stop)
                {
                    _start = false;
                    _startElapsedTime = _elapsedTime;
                    _elapsedTime = 0;
                    _distanceTraveled = 0;
                }
            }
        }

        //Converts patientid into json string
        public static void SendPatientId()
        {
            LoginMessage login = new LoginMessage()
            {
                id = "server/login",
                data = new SpecificLoginMessage()
                {
                    patientId = _username
                }
            };
            Console.WriteLine(login);
            SendData(JsonConvert.SerializeObject(login));
        }

        //Listens to incoming messages from server and converts to string
        public static void ReadJsonMessage(TcpClient client)
        {
            if (_connected)
            {
                var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
                {
                    string message = "";


                    while (stream.Peek() != -1)
                    {
                        message += stream.ReadLine();
                    }


                    Console.WriteLine(message);
                    MessageHandler(message);
                }
            }
        }

        //Receives messages and takes action
        public static void MessageHandler(string message)
        {
            dynamic jsonMessage = JsonConvert.DeserializeObject(message);
            string id = "";
            try
            {
                id = jsonMessage.id;
            }
            catch
            {
                Console.WriteLine("can't find id in message:" + message);
            }
            switch (id)
            {


                //server connected with client
                case "client/connected":
                    Console.WriteLine("Server heeft testbericht ontvangen");
                    break;

                //server received patientid
                case "client/login":
                    if (jsonMessage.newAccount == true)
                    {
                        Console.WriteLine("Account aangemaakt met pati�ntnummer " + _username);
                    }
                    else
                    {
                        Console.WriteLine("Ingelogd met pati�ntnummer " + _username);
                    }
                    break;

                //server received live data
                case "client/received":
                    Console.WriteLine("Server heeft data ontvangen");
                    break;

                //doctor pressed emergency stop
                case "server/emergencyStop":
                    Console.WriteLine("Dokter drukt op de noodstop");

                    break;

                //doctor starts a session
                case "server/startSession":
                    _start = true;
                    Console.WriteLine("Dokter start een session");
                    break;

                //doctor stops the session
                case "server/endSession":
                    _stop = true;
                    Console.WriteLine("Dokter start een session");
                    break;

                //doctor sends a message
                case "server/sent":
                    Console.WriteLine("Received message:" + jsonMessage.message);
                    break;

                //error
                default:
                    Console.WriteLine("received unknown message:\n" + message);
                    break;
            }
        }

    }
}

