using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.AccessControl;
using System.Threading;

namespace FietsDemo;

public static class Simulator
{
        public static string Output = "";

        private static int _totalSpeeds = 0;
        private static int _generatedSpeeds = 0;

        private const int Sync = 164;
        private const int MsgLength = 9;
        private const int MsgId = 78;
        private const int Channel = 5;
        private const int DataPageNumber = 16;
        private const int EquipmentType = 152;
        private const int SpeedLsb = 0;
        private const int SpeedMsb = 0;
        private const int Field = 32;
        private const int Checksum = 0;
        
        private static int _time = 0;
        private static double _distanceTraveled = 0;
        private static int _speed = -1;
        private static int _heartRate = 40;
        private static bool _heartrateRising = true;
        private static bool _speedRising = true;

        private static readonly Random Random = new();

        private static readonly string[] RandomCodes =
        {
            "A4-09-4E-05-19-0D-00-D7-20-00-60-20-45",
            "A4-09-4E-05-19-0E-00-D7-20-00-60-20-46",
            "A4-09-4E-05-19-0F-00-D7-20-00-60-20-47",
            "A4-09-4E-05-19-10-00-D7-20-00-60-20-58",
            "A4-09-4E-05-19-11-00-D7-20-00-60-20-59",
            "A4-09-4E-05-19-12-00-D7-20-00-60-20-5A",
            "A4-09-4E-05-19-13-00-D7-20-00-60-20-5B",
            "A4-09-4E-05-19-14-00-D7-20-00-60-20-5C",
            "A4-09-4E-05-19-15-00-D7-20-00-60-20-5D"
        };

        static List<double> DataItems = new()
        {
            Sync, MsgLength, MsgId, Channel, DataPageNumber, EquipmentType, _time, _distanceTraveled, SpeedLsb, SpeedMsb, _heartRate, Field, Checksum
        };

        /// <summary>
        /// This function simulates the data that would be sent from the bike to the computer
        /// </summary>
        /// <returns>
        /// An array of integers.
        /// </returns>
        public static int[] SimulateGeneralData()
        {
            Tuple<short, short> speeds = SetSpeed() ?? throw new ArgumentNullException("SetSpeed()");
            DataItems[8] = speeds.Item1;
            DataItems[9] = speeds.Item2;
            _heartRate = SetHeartRate(_heartRate);
            DataItems[10] = _heartRate;
            DataItems[7] = CalculateDistanceTraveled();
            _time++;
            DataItems[6] = _time;
            DataItems[12] = CalculateChecksum(DataItems);
            

            int[] values = new int[DataItems.Count];

            for (int i = 0; i < DataItems.Count; i++)
            {
                values[i] = (int)DataItems[i];
            }

            return values;
        }

        /// <summary>
        /// It returns an array of two integers, the first being 0 and the second being a random number between 75 and 125
        /// </summary>
        /// <returns>
        /// An array of two integers.
        /// </returns>
        public static int[] SimulateHeartRate()
        {
            int rate = new Random().Next(50) + 75;
            return new[]{0, rate };
        }

        /// <summary>
        /// It takes a random string from the RandomCodes array, splits it into an array of strings, converts each string to
        /// an integer, and returns the array of integers
        /// </summary>
        /// <returns>
        /// An array of integers.
        /// </returns>
        public static int[] SimulateBikeData()
        {
            string[] data = RandomCodes[Random.Next(8)].Split('-');
            int[] values = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
            }

            return values;
        }

        /// <summary>
        /// It generates a random speed value between 0 and 10, and returns it as a tuple of two bytes
        /// </summary>
        /// <returns>
        /// The speed of the motor.
        /// </returns>
        private static Tuple<short, short> SetSpeed()
        {
            if (_speedRising)
            {
                _speed += 1;
                if (_speed >= 10)
                {
                    _speedRising = false;
                }
            }
            else
            {
                _speed -= 1;
                if (_speed <= 0)
                {
                    _speedRising = true;
                }
            }

            byte Msb = (byte)(_speed & 0xFF);
            byte Lsb = (byte) ((_speed >> 8) & 0xFF);

            while (Lsb > Math.Pow(2, 8))
            {
                Lsb -= (byte) Math.Pow(2, 8);
            }

            _totalSpeeds += _speed;
            _generatedSpeeds++;
            return new Tuple<short, short>(Lsb, Msb);
        }

        /// <summary>
        /// If the heart rate is rising, add 10 to the heart rate. If the heart rate is not rising, subtract 10 from the
        /// heart rate. If the heart rate is greater than or equal to 150, set the heart rate to rising. If the heart rate
        /// is less than or equal to 50, set the heart rate to not rising
        /// </summary>
        /// <param name="heartRate">The current heart rate.</param>
        /// <returns>
        /// The heart rate is being returned.
        /// </returns>
        private static int SetHeartRate(int heartRate)
        {
            if (_heartrateRising)
            {
                heartRate += 10;
                if(heartRate >= 150)
                {
                    _heartrateRising = false;
                }
            }
            else
            {
                heartRate -= 10;
                if(heartRate <= 50)
                {
                    _heartrateRising = true;
                }
            }

            return heartRate;
        }

        /// <summary>
        /// It takes the distance traveled, adds the speed times 0.25, and then makes sure the distance traveled is less
        /// than 256
        /// </summary>
        /// <returns>
        /// The distance traveled is being returned.
        /// </returns>
        private static double CalculateDistanceTraveled()
        {
            _distanceTraveled += _speed * 0.25;
            while (_distanceTraveled > Math.Pow(2, 8))
            {
                _distanceTraveled -= Math.Pow(2, 8);
            }
            return _distanceTraveled;
        }

        /// <summary>
        /// It takes a list of doubles, adds them together, and returns the sum modulo 256
        /// </summary>
        /// <param name="dataItems">The list of data items to be sent.</param>
        /// <returns>
        /// The checksum is being returned.
        /// </returns>
        private static int CalculateChecksum(List<double> dataItems)
        {
            int checksum = 0;
            foreach(double item in dataItems)
            {
                checksum += (int) item;
            }
            while (checksum > Math.Pow(2, 8))
            {
                checksum -= (int)Math.Pow(2, 8);
            }
            

            return checksum;
        }
}