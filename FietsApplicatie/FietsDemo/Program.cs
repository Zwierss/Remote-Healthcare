using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo
{
    class Program
    {
        public static Task Main(string[] args)
        {
            /*
            int errorCode = 0;
            BLE bleBike = new BLE();
            BLE bleHeart = new BLE();
            Thread.Sleep(1000); // We need some time to list available devices

            // List available devices
            List<string> bleBikeList = bleBike.ListDevices();
            Console.WriteLine("Devices found: ");
            foreach (var name in bleBikeList)
            {
                Console.WriteLine($"Device: {name}");
            }

            // Connecting
            errorCode = errorCode = await bleBike.OpenDevice("Tacx Flux 01140");
            // __TODO__ Error check

            var services = bleBike.GetServices;
            foreach(var service in services)
            {
                //Console.WriteLine($"Service: {service.}");
            }

            // Set service
            errorCode = await bleBike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
            // __TODO__ error check

            // Subscribe
            bleBike.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            errorCode = await bleBike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");

            // Heart rate
            errorCode =  await bleHeart.OpenDevice("Decathlon Dual HR");

            await bleHeart.SetService("HeartRate");

            bleHeart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            await bleHeart.SubscribeToCharacteristic("HeartRateMeasurement");
            */
            
            Bike bike = new Bike();
            HeartRate heart = new HeartRate();

            Console.WriteLine("Trying connection with devices");
            bool bikeConnection = bike.MakeConnection().Result;

            if (!bikeConnection)
            {
                bool hearRateConnection = heart.MakeConnection().Result;

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
            /*
            string[] data = BitConverter.ToString(e.Data).Split('-');
            int[] values = new int[data.Length];
            
            for (int i = 0; i < data.Length; i++)
            {
                values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
            }

            for (int i = 0; i < values.Length; i++)
            {
                Console.Write(values[i] + "-");
            }
            
            Console.WriteLine("");
            */

            string[] data = BitConverter.ToString(e.Data).Split('-');
            int[] values = new int[data.Length];
            
            for (int i = 0; i < data.Length; i++)
            {
                values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
            }

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
        
        private static void PrintGeneralData(int[] values)
        {
            Console.WriteLine("Received General Data");
            Console.WriteLine("-----------");
            Console.WriteLine("Equipment Type: " + values[6]);
            Console.WriteLine("Elapsed Time: " + (values[7])+ " seconds");
            Console.WriteLine("Distance Traveled: " + values[8] + " meters");
            Console.WriteLine("Speed LSB: " + values[9] + " m/s");
            Console.WriteLine("Speed MSB: " + values[10] + " m/s");
            Console.WriteLine("Heart Rate: " + values[11] + " bpm");
        }

        private static void PrintBikeData(int[] values)
        {
            Console.WriteLine("Received Bike Data");
            Console.WriteLine("-----------");
            Console.WriteLine("Event Count: " + values[6]);
            Console.WriteLine("Instantaneous Cadence: " + values[7] + " rpm");
            Console.WriteLine("Accumulated Power LSB: " + values[8] + " W");
            Console.WriteLine("Accumulated Power MSB: " + values[9] + " W");
            Console.WriteLine("Instantaneous Power LSB: " + values[10] + " W");
            string splitted = Convert.ToString(values[11], 2);
            Console.WriteLine("Instantaneous Power MSB: " + Convert.ToInt32(splitted.Substring(0,4), 2) + " W");
            Console.WriteLine("Trainer Status: " + Convert.ToInt32(splitted.Substring(4,4), 2));
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
            for (int i = 0; i < values.Length; i++)
            {
                Console.Write(values[i] + "bpm" );
            }
            Console.WriteLine();
            Console.WriteLine("-----------");
        }
    }
}
