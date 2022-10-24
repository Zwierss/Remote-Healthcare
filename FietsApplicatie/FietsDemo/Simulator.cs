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

        private static readonly Random Random = new Random();

        private static readonly string[] RandomCodes = new string[]
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

        static List<double> DataItems = new List<double>()
        {
            Sync, MsgLength, MsgId, Channel, DataPageNumber, EquipmentType, _time, _distanceTraveled, SpeedLsb, SpeedMsb, _heartRate, Field, Checksum
        };

        public static int[] SimulateGeneralData()
        {
            //Speed++;
            Tuple<short, short> speeds = SetSpeed() ?? throw new ArgumentNullException("SetSpeed()");
            DataItems[8] = speeds.Item1;
            DataItems[9] = speeds.Item2;
            _heartRate = SetHeartRate(_heartRate);
            DataItems[10] = _heartRate;
            DataItems[7] = CalculateDistanceTraveled();
            IncreaseTime();
            DataItems[6] = _time;
            DataItems[12] = CalculateChecksum(DataItems);

            for(int i = 0; i < 13; i++)
            {
                string dataString = Convert.ToString((short)DataItems[i], 16 )+ "-";
            }

            int[] values = new int[DataItems.Count];

            for (int i = 0; i < DataItems.Count; i++)
            {
                values[i] = (int)DataItems[i];
            }

            return values;
        }

        public static int[] SimulateHeartRate()
        {
            int rate = new Random().Next(50) + 75;
            return new[]{0, rate };
        }

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

        private static double CalculateDistanceTraveled()
        {
            _distanceTraveled += _speed * 0.25;
            while (_distanceTraveled > Math.Pow(2, 8))
            {
                _distanceTraveled -= Math.Pow(2, 8);
            }
            return _distanceTraveled;
        }

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

        private static void IncreaseTime()
        {
            _time++;
        }
}