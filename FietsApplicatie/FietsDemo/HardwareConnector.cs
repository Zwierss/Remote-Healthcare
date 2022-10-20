using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo
{
    public static class HardwareConnector
    {

        private static IClientCallback _client;
        private static Bike _bike;
        public static bool Connected { get; set; } = false;

        public static async Task SetupHardware(IClientCallback client, string bikeSerial)
        {
            new Thread(RunSimulation).Start();
            _client = client;

            // _bike = new Bike();
            // _bike.Serial = bikeSerial;
            // HeartRate heart = new HeartRate();
            //
            // Console.WriteLine("Trying connection with devices");
            // bool bikeConnection = _bike.MakeConnection().Result;
            // if(!bikeConnection) return;
            //
            // bool hearRateConnection = heart.MakeConnection().Result;
            // if(!hearRateConnection) return;

            Connected = true;
            Console.Read();
        }

        private static void SetBikeSerial(string serial)
        {
            _bike.Serial = serial;
        }

        private static void RunSimulation()
        {
            while (true)
            {
                Thread.Sleep(500);
                int[] values = Simulator.SimulateGeneralData();
                PrintGeneralData(values);
                Thread.Sleep(500);
                int[] heartData = Simulator.SimulateHeartRate();
                PrintHeartData(heartData);
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

        public static void SetResistance(byte resistance)
        {
            _ = _bike.SetResistance(resistance);
        }
        private static void PrintGeneralData(IReadOnlyList<int> values)
        {
            _client.OnNewBikeData(values);
            Console.WriteLine("Received General Data");
            Console.WriteLine("-----------");
            Console.WriteLine("Equipment Type: " + values[5]);
            Console.WriteLine("Elapsed Time: " + (values[6])+ " seconds");
            Console.WriteLine("Distance Traveled: " + values[7] + " meters");
            double speed = (values[8] + values[9] * 255) * 0.001;
            Console.WriteLine("Speed: " + speed + " m/s");

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
            _client.OnNewHeartrateData(values);
            Console.WriteLine("Received Heart Rate Data");
            Console.WriteLine("-----------");
            Console.WriteLine(values[1] + " bpm");
            Console.WriteLine("-----------");
        }
    }
}
