using System;
using System.Collections.Generic;
using Avans.TI.BLE;

namespace FietsDemo;

public class DataCollector
{

    public DataCollector()
    {
    }

    public void GetMessages(object sender, BLESubscriptionValueChangedEventArgs e)
    {
        string[] data = BitConverter.ToString(e.Data).Split('-');
        int[] values = new int[data.Length];
            
        for (int i = 0; i < data.Length; i++)
        {
            values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
        }

        switch (data[5])
        {
            case "0x10":
                PrintGeneralData(values);
                break;
            case "0x19":
                PrintBikeData(values);
                break;
        }
    }

    private void PrintGeneralData(int[] values)
    {
        Console.WriteLine("Received General Data");
        Console.WriteLine("-----------");
        Console.WriteLine("Equipment Type: " + values[6]);
        Console.WriteLine("Elapsed Time: " + values[7]);
        Console.WriteLine("Distance Traveled: " + values[8]);
        Console.WriteLine("Speed LSB: " + values[9]);
        Console.WriteLine("Speed MSB: " + values[10]);
        Console.WriteLine("Heart Rate: " + values[11]);
    }

    private void PrintBikeData(int[] values)
    {
        Console.WriteLine("Received Bike Data");
        Console.WriteLine("-----------");
        Console.WriteLine("Event Count: " + values[6]);
        Console.WriteLine("Instantaneous Cadence: " + values[7]);
        Console.WriteLine("Accumulated Power LSB: " + values[8]);
        Console.WriteLine("Accumulated Power MSB: " + values[9]);
        Console.WriteLine("Instantaneous Power LSB: " + values[10]);
        string splitted = Convert.ToString(values[11], 2);
        Console.WriteLine("Instantaneous Power MSB: " + Convert.ToInt32(splitted.Substring(0,4), 2));
        Console.WriteLine("Trainer Status: " + Convert.ToInt32(splitted.Substring(4,4), 2));
    }
}