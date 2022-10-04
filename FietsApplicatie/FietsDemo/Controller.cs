using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;
using VirtualReality;

namespace FietsDemo
{
    static class Controller
    {

        private static VRClient _vr;
        
        public static Task SetupHardware()
        {

            //RunSimulation();
            
            Bike bike = new Bike();
            HeartRate heart = new HeartRate();
            
            Console.WriteLine("Trying connection with devices");
            bool bikeConnection = bike.MakeConnection().Result;
            while(!bikeConnection) Thread.Sleep(1000);
            bool hearRateConnection = heart.MakeConnection().Result;
            
            if (!bikeConnection)
            {
                if(!hearRateConnection){
                    Console.WriteLine("Could not connect with the devices. DO you want to connect with the simulator? (y/n)");
                    string input = Console.ReadLine();
                    if (input == "y")
                    {
                        Console.WriteLine("Starting Simulation");
                    }
                    else
                    {
                        Console.WriteLine("Closing down application");
                    }
                }
            }
            new Thread(CreateVR).Start();
            Console.Read();
            return Task.CompletedTask;
        }

        private static void RunSimulation()
        {
            while (true)
            {
                Thread.Sleep(500);
                int[] values = Simulator.SimulateGeneralData();
                PrintGeneralData(values);
                Thread.Sleep(500);
                int[] bikeData = Simulator.simulateBikeData();
                PrintBikeData(bikeData);
            }
        }

        private static void CreateVR()
        {
            _vr = new VRClient();
            Thread.Sleep(1000);
            
#pragma warning disable CS4014
            _vr.StartConnection();
#pragma warning restore CS4014

            while (true)
            {
                Thread.Sleep(10);
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
        
        private static void PrintGeneralData(IReadOnlyList<int> values)
        {
            Console.WriteLine("Received General Data");
            Console.WriteLine("-----------");
            Console.WriteLine("Equipment Type: " + values[5]);
            Console.WriteLine("Elapsed Time: " + (values[6])+ " seconds");
            Console.WriteLine("Distance Traveled: " + values[7] + " meters");
            double speed = (values[8] + values[9] * 255) * 0.001;
            Console.WriteLine("Speed: " + speed + " m/s");
            if (_vr.IsSet)
            {
                _vr.UpdateBikeSpeed(speed * 3.6);
            }

            Console.WriteLine("Heart Rate: " + values[10] + " bpm");
            Console.WriteLine("-----------");
        }

        private static void PrintBikeData(IReadOnlyList<int> values)
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

        private static void PrintHeartData(IReadOnlyList<int> values)
        {
            Console.WriteLine("Received Heart Rate Data");
            Console.WriteLine("-----------");
            Console.WriteLine(values[1] + " bpm");
            Console.WriteLine("-----------");
        }
    }
}
