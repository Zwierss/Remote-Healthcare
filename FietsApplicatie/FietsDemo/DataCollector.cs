using System;
using Avans.TI.BLE;

namespace FietsDemo;

public class DataCollector
{
    private static DataCollector dc;

    public DataCollector()
    {
        
    }

    public void SetSubscriptionValue(object sender, BLESubscriptionValueChangedEventArgs e)
    {
            
        string[] data = BitConverter.ToString(e.Data).Split('-');
        int[] values = new int[data.Length];
            
        for (int i = 0; i < data.Length; i++)
        {
            values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
        }
    }
}