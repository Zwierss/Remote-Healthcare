using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;

namespace FietsDemo;

public class Simulator
{
    static string output = "";

        static int TotalSpeeds = 0;
        static int GeneratedSpeeds = 0;

        static int Sync = 164;
        static int MsgLenth = 9;
        static int MsgId = 78;
        static int Channel = 5;
        static int DataPageNumber = 16;
        static int EquipmentType = 152;
        static int Time = 0;
        static double DistanceTraveled = 0;
        static int Speed = -1;
        static int SpeedLsb = 0;
        static int SpeedMsb = 0;
        static int HeartRate = 40;
        static int Field = 32;
        static int Checksum = 0;

        static bool HeartrateRising = true;
        static bool SpeedRising = true;

        static Random random = new Random();

        private static string[] _randomCodes = new string[]
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
            Sync, MsgLenth, MsgId, Channel, DataPageNumber, EquipmentType, Time, DistanceTraveled, SpeedLsb, SpeedMsb, HeartRate, Field, Checksum
        };

        public static int[] SimulateGeneralData()
        {
            //Speed++;
            Tuple<short, short> Speeds = SetSpeed();
            DataItems[8] = Speeds.Item1;
            DataItems[9] = Speeds.Item2;
            HeartRate = SetHeartRate(HeartRate);
            DataItems[10] = HeartRate;
            DataItems[7] = CalculateDistanceTraveled();
            IncreaseTime();
            DataItems[6] = Time;
            DataItems[12] = CalculateChecksum(DataItems);

            string data = "";
            for(int i = 0; i < 13; i++)
            {
                string dataString = Convert.ToString((short)DataItems[i], 16 )+ "-";
                data += $"{(dataString.ToString().Length == 1 ? "0" : "")}" + dataString;
            }

            int[] values = new int[DataItems.Count];

            for (int i = 0; i < DataItems.Count; i++)
            {
                values[i] = (int)DataItems[i];
            }

            return values;
        }

        public static int[] simulateBikeData()
        {
            string[] data = _randomCodes[random.Next(8)].Split('-');
            int[] values = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
            }

            return values;
        }

        public static Tuple<short, short> SetSpeed()
        {
            if (SpeedRising)
            {
                Speed += 1;
                if (Speed >= 10)
                {
                    SpeedRising = false;
                }
            }
            else
            {
                Speed -= 1;
                if (Speed <= 0)
                {
                    SpeedRising = true;
                }
            }

            byte Msb = (byte)(Speed & 0xFF);
            byte Lsb = (byte) ((Speed >> 8) & 0xFF);

            while (Lsb > Math.Pow(2, 8))
            {
                Lsb -= (byte) Math.Pow(2, 8);
            }

            TotalSpeeds += Speed;
            GeneratedSpeeds++;
            return new Tuple<short, short>(Lsb, Msb);
        }
        
        public static int SetHeartRate(int HeartRate)
        {
            //Random random = new Random();
            //HeartRate = random.Next(50, 120);
            /*
            while (HeartRate > Math.Pow(2, 8))
            {
                HeartRate -= (byte)Math.Pow(2, 8);
            }
            */

            if (HeartrateRising)
            {
                HeartRate += 10;
                if(HeartRate >= 150)
                {
                    HeartrateRising = false;
                }
            }
            else
            {
                HeartRate -= 10;
                if(HeartRate <= 50)
                {
                    HeartrateRising = true;
                }
            }

            return HeartRate;
        }

        public static double CalculateDistanceTraveled()
        {
            DistanceTraveled += Speed * 0.25;
            while (DistanceTraveled > Math.Pow(2, 8))
            {
                DistanceTraveled -= Math.Pow(2, 8);
            }
            return DistanceTraveled;
        }

        public static int CalculateAverageSpeed()
        {
            int AverageSpeed = TotalSpeeds / GeneratedSpeeds;

            return AverageSpeed;
        }

        public static int CalculateChecksum(List<double> DataItems)
        {
            int Checksum = 0;
            foreach(double item in DataItems)
            {
                Checksum += (int) item;
            }
            while (Checksum > Math.Pow(2, 8))
            {
                Checksum -= (int)Math.Pow(2, 8);
            }
            

            return Checksum;
        }

        public static int IncreaseTime()
        {
            return Time++;
        }

    public static void reset()
    {
        Sync = 164;
        MsgLenth = 9;
        MsgId = 78;
        Channel = 5;
        DataPageNumber = 16;
        EquipmentType = 152;
        Time = 0;
        DistanceTraveled = 0;
        Speed = -1;
        SpeedLsb = 0;
        SpeedMsb = 0;
        HeartRate = 40;
        Field = 32;
        Checksum = 0;
    }
}