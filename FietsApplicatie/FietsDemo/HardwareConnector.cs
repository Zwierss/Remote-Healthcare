using System;
using System.Collections.Generic;
using System.Threading;
using Avans.TI.BLE;

namespace FietsDemo
{
    public static class HardwareConnector
    {

        private static IHardwareCallback _client;
        private static Bike _bike;
        private static HeartRate _heart;
        private static Thread _timer;
        private static Thread _simThread;
        private static bool _sim;
        public static bool Connected { get; set; } = false;
        public static int Time { get; set; } = 0;

        public static void SetupHardware(IHardwareCallback client, string bikeSerial, bool sim)
        {
            _client = client;
            _sim = sim;
            if (sim)
            {
                _simThread = new Thread(RunSimulation);
                _simThread.Start();
            }
            else
            {
                _bike = new Bike();
                _bike.Serial = bikeSerial;
                _heart = new HeartRate();
            
                Console.WriteLine("Trying connection with devices");
                bool bikeConnection = _bike.MakeConnection().Result;
                while (!bikeConnection)
                {
                    Thread.Sleep(1000);
                    bikeConnection = _bike.MakeConnection().Result;
                }
                bool hearRateConnection = _heart.MakeConnection().Result;
                while (!hearRateConnection)
                {
                    Thread.Sleep(1000);
                    hearRateConnection = _bike.MakeConnection().Result;
                }
            }
            _client.OnSuccessfulConnect();
            Connected = true;
            Console.Read();
        }

        public static void Stop()
        {
            if (_sim)
            {
                _simThread.Abort();
            }
            else
            {
                _bike.Disconnect();
                _heart.Disconnect();
            }
        }

        private static void RunSimulation()
        {
            while (true)
            {
                Thread.Sleep(500);
                int[] values = Simulator.SimulateGeneralData();
                _client.OnNewBikeData(values);
                //PrintGeneralData(values);
                Thread.Sleep(500);
                int[] heartData = Simulator.SimulateHeartRate();
                _client.OnNewHeartrateData(heartData);
                //PrintHeartData(heartData);
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
                _client.OnNewHeartrateData(values);
                //PrintHeartData(values);
            }
            else
            {
                switch (data[4])
                {
                    case "10":
                        _client.OnNewBikeData(values);
                        //PrintGeneralData(values);
                        break;
                    case "19":
                        //PrintBikeData(values);
                        break;
                }
            }
        }

        public static void StartSessionTimer()
        {
            _timer = new Thread(SessionTimer);
            _timer.Start();
            Time = 0;
        }

        public static void StopSessionTimer()
        {
            _timer?.Abort();
        }

        private static void SessionTimer()
        {
            while (true)
            {
                Time++;
                Thread.Sleep(1000);
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
