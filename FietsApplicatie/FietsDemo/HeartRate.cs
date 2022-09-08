using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo;

public class HeartRate
{

    private readonly BLE _ble;
    private DataCollector _collector;
    
    public HeartRate(DataCollector collector)
    {
        _ble = new BLE();
        _collector = new DataCollector();
    }

    public async Task<bool> MakeConnection()
    {
        int errorCode = 1;
        Thread.Sleep(1000);

        errorCode = await _ble.OpenDevice("Decathlon Dual HR");
        if (errorCode == 1) return false;
        
        await _ble.SetService("HeartRate");
        _ble.SubscriptionValueChanged += _collector.SetSubscriptionValue;
        await _ble.SubscribeToCharacteristic("HeartRateMeasurement");

        return true;
    }
}