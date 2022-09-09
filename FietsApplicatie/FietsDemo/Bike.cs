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
        Thread.Sleep(1000);
        
        int errorCode = await _ble.OpenDevice("Tacx Flux 01140");
        if (errorCode == 1) return false;
        
        errorCode = await _ble.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
        if (errorCode == 1) return false;

        _ble.SubscriptionValueChanged += _collector.GetMessages;
        errorCode = await _ble.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");
        if (errorCode == 1) return false;
        
        return true;
    }
}