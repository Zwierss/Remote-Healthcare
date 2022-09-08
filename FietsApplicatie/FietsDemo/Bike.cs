using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo;

public class Bike
{

    private readonly BLE _ble;
    private DataCollector _collector;

    public Bike(DataCollector collector)
    {
        _ble = new BLE();
        this._collector = collector;
    }

    public async Task<bool> MakeConnection()
    {
        int errorCode = 1;
        Thread.Sleep(1000);
        
        errorCode = await _ble.OpenDevice("Tacx Flux 01140");
        if (errorCode == 1) return false;
        
        List<BluetoothLEAttributeDisplay> services = _ble.GetServices;
        errorCode = await _ble.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
        if (errorCode == 1) return false;

        _ble.SubscriptionValueChanged += _collector.SetSubscriptionValue;
        errorCode = await _ble.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");
        if (errorCode == 1) return false;
        
        return true;
    }

    private void SetSubscriptionValue(object sender, BLESubscriptionValueChangedEventArgs e)
    {
            
        string[] data = BitConverter.ToString(e.Data).Split('-');
        int[] values = new int[data.Length];
            
        for (int i = 0; i < data.Length; i++)
        {
            values[i] = int.Parse(data[i],System.Globalization.NumberStyles.HexNumber);
        }
    }

    public BLE GetBle()
    {
        return _ble;
    }
}