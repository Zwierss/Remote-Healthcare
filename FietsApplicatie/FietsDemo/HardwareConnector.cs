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

        /// <summary>
        /// This function is used to connect to the hardware. It takes in a callback function, a bike serial number, and a
        /// boolean value to determine if the hardware is being simulated. If the hardware is being simulated, a thread is
        /// created to run the simulation. If the hardware is not being simulated, the bike and heart rate monitor are
        /// created and connected to
        /// </summary>
        /// <param name="IHardwareCallback">This is an interface that the client must implement. It is used to send data
        /// back to the client.</param>
        /// <param name="bikeSerial">The serial number of the bike you want to connect to.</param>
        /// <param name="sim">true if you want to run the simulation, false if you want to connect to the hardware</param>
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
        }

        /// <summary>
        /// If we're in simulation mode, abort the simulation thread. Otherwise, disconnect the bike and heart rate monitor
        /// </summary>
        /// <returns>
        /// The method is returning the current session time.
        /// </returns>
        public static void Stop()
        {
            if (_sim)
            {
                if(_simThread == null) return;
                _simThread.Abort();
            }
            else
            {
                if (_bike == null || _heart == null) return;
                _bike.Disconnect();
                _heart.Disconnect();
            }
            StopSessionTimer();
        }

        /// <summary>
        /// It sets the resistance of the bike
        /// </summary>
        /// <param name="resistance">0-255</param>
        /// <returns>
        /// The resistance level of the bike.
        /// </returns>
        public static void SetResistance(byte resistance)
        {
            if (_sim)
            {
                Console.WriteLine("did no set resistance because it is a simulation, but it should've been " + resistance);
                return;
            }
            _bike.SetResistance(resistance);
        }
        
        /// <summary>
        /// The function runs in an infinite loop, and every 500 milliseconds it calls the `SimulateGeneralData` and
        /// `SimulateHeartRate` functions from the `Simulator` class, and then calls the `OnNewBikeData` and
        /// `OnNewHeartrateData` functions from the `Client` class
        /// </summary>
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

        /// <summary>
        /// It takes the data from the BLE subscription and converts it to an array of integers
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="BLESubscriptionValueChangedEventArgs"></param>
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

        /// <summary>
        /// It creates a new thread that runs the SessionTimer function
        /// </summary>
        public static void StartSessionTimer()
        {
            _timer = new Thread(SessionTimer);
            _timer.Start();
            Time = 0;
        }

        /// <summary>
        /// It stops the timer.
        /// </summary>
        public static void StopSessionTimer()
        {
            _timer?.Abort();
        }

        /// <summary>
        /// It's a while loop that runs forever, and every second it increments a variable called Time
        /// </summary>
        private static void SessionTimer()
        {
            while (true)
            {
                Time++;
                Thread.Sleep(1000);
            }
        }
    }
}
