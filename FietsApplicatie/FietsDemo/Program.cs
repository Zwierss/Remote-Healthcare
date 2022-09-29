using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FietsDemo
{
    class Program
    {
        private static TcpClient _client;
        private static NetworkStream _stream;
        private static String _username;
        public static Task Main(string[] args)
        {

            //runSimulation();
            Console.WriteLine("Client started");
            _client = new TcpClient();
            _client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);


            Bike bike = new Bike();
            HeartRate heart = new HeartRate();
            Client client = new Client();

            Console.WriteLine("Trying connection with devices");
            bool bikeConnection = bike.MakeConnection().Result;

            //while(!bikeConnection) Thread.Sleep(1000);
            bool hearRateConnection = heart.MakeConnection().Result;

            //bool clientConnection = _client.MakeConnection().Result;
            JObject jObject = new JObject();


            if (!bikeConnection)
            {
                if (!hearRateConnection)
                {
                    Console.WriteLine("Could not connect with the devices. DO you want to connect with the simulator? (y/n)");
                    string input = Console.ReadLine();
                    if (input == "y")
                    {
                        Console.WriteLine("Starting Simulation");
                        runSimulation();
                    }
                    else
                    {
                        Console.WriteLine("Closing down application");
                        return Task.CompletedTask;
                    }
                }
            }

            Console.Read();
            return Task.CompletedTask;
        }

        public static void runSimulation()
        {
            bool running = true;
            while (running)
            {
                Thread.Sleep(250);
                int[] values = Simulator.SimulateGeneralData();
                PrintGeneralData(values);
                Thread.Sleep(250);
                int[] bikeData = Simulator.simulateBikeData();
                PrintBikeData(bikeData);
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
                        PrintGeneralData(values);
                        sendData(values);
                        break;
                    case "19":
                        PrintBikeData(values);
                        sendData(values);
                        break;
                }
            }



        }

        private static void PrintGeneralData(int[] values)
        {
            Console.WriteLine("Received General Data");
            Console.WriteLine("-----------");
            Console.WriteLine("Equipment Type: " + values[5]);
            Console.WriteLine("Elapsed Time: " + (values[6]) + " seconds");
            Console.WriteLine("Distance Traveled: " + values[7] + " meters");
            Console.WriteLine("Speed: " + (values[9] + (values[8] << 8) * 0.001) + " m/s");
            Console.WriteLine("Heart Rate: " + values[10] + " bpm");
            Console.WriteLine("-----------");

        }

        private static void PrintBikeData(int[] values)
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
            Console.WriteLine("Received Heart Rate Data");
            Console.WriteLine("-----------");
            Console.WriteLine(values[1] + " bpm");
            Console.WriteLine("-----------");
        }

        private static void OnConnect(IAsyncResult ar)
        {
            _client.EndConnect(ar);
            Console.WriteLine("Verbonden!");

        }
        private static void convertData(int[] values)
        {
            JsonData data = new JsonData
            {
                Username = _username,
                Speed = values[9] + (values[8] << 8) * 0.001,
                Heartrate = values[10]
            };
            sendData(data);
        }

        private static void sendData(JsonData ob)
        {

        }

        private static void receiveData()
        { 

        }
    }
}
