using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;


namespace FietsDemo
{
    class Program
    {
        private static TcpClient _client;
        private static NetworkStream _stream;
        public static Task Main(string[] args)
        {

            //runSimulation();
            Console.WriteLine("Client started");
            client = new TcpClient();
            client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);


            Bike bike = new Bike();
            HeartRate heart = new HeartRate();

            Console.WriteLine("Trying connection with devices");
            bool bikeConnection = bike.MakeConnection().Result;
            Console.WriteLine("c");
            //while(!bikeConnection) Thread.Sleep(1000);
            Console.WriteLine("d");
            bool hearRateConnection = heart.MakeConnection().Result;
            Console.WriteLine("e");

            bool clientConnection = client.MakeConnection().Result;


            if (!bikeConnection)
            {
                if(!hearRateConnection){
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
                        Console.WriteLine("b");
                        return Task.CompletedTask;
                    }
                }
            }

            Console.Read();
            Console.WriteLine("a");
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
                values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
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
                        break;
                    case "19":
                        PrintBikeData(values);
                        break;
                }
            }

            
            
        }
        
        private static void PrintGeneralData(int[] values)
        {
            Console.WriteLine("Received General Data");
            Console.WriteLine("-----------");
            Console.WriteLine("Equipment Type: " + values[5]);
            Console.WriteLine("Elapsed Time: " + (values[6])+ " seconds");
            Console.WriteLine("Distance Traveled: " + values[7] + " meters");
            Console.WriteLine("Speed: " + (values[9] + (values[8] << 8) * 0.001) + " m/s");
            Console.WriteLine("Heart Rate: " + values[10] + " bpm");
            Console.WriteLine("-----------");

            JsonFile jsonfile = new JsonFile
            {

            }

        }

        private static void PrintBikeData(int[] values)
        {
            Console.WriteLine("Received Bike Data");
            Console.WriteLine("-----------");
            Console.WriteLine("Event Count: " + values[5]);
            Console.WriteLine("Instantaneous Cadence: " + values[6] + " rpm");
            Console.WriteLine("Accumulated Power LSB: " + values[7] + " W");
            Console.WriteLine("Accumulated Power MSB: " + values[8] + " W");
            Console.WriteLine("Instantaneous Power LSB: " + values[9] + " W");
            string splitted = Convert.ToString(values[10], 2);
            Console.WriteLine("Instantaneous Power MSB: " + Convert.ToInt32(splitted.Substring(0,4), 2) + " W");
            Console.WriteLine("Trainer Status: " + Convert.ToInt32(splitted.Substring(3,4), 2));
            Console.WriteLine("-----------");
        }

        public static void BleHeartRate_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            string[] data = BitConverter.ToString(e.Data).Split('-');
            int[] values = new int[data.Length];
            
            for (int i = 0; i < data.Length; i++)
            {
                values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
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

            Console.WriteLine(ReadTextMessage(client));
            Console.WriteLine("Voer cliŽntnummer in");
            username = Console.ReadLine();
            WriteTextMessage(client, username);
            Console.WriteLine(ReadTextMessage(client));

            stream = client.GetStream();

            handlingServer();
        }
        public static void SendData(JsonFile jsonFile)
        {

        }

        private static void receiveData()
        {

        }
    }
}
