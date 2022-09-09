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
        public static async Task Main(string[] args)
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

            DataCollector collector = new DataCollector();
            Bike bike = new Bike(collector);
            HeartRate heart = new HeartRate(collector);
            
            if (!bike.MakeConnection().Result)
            {
                Console.WriteLine("Could not connect with bike");
            }

            if (!heart.MakeConnection().Result)
            {
                Console.WriteLine("Could not connect with heart rate device");
            }

            Console.Read();
        }
        
        /*
        private static void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            
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

            //Console.WriteLine(BitConverter.ToString(e.Data));
        }
        */
    }
}
