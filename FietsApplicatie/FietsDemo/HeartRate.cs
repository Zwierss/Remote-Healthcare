using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo;

public class HeartRate
{

    private readonly BLE _ble;

    public HeartRate()
    {
        _ble = new BLE();
    }

    public async Task<bool> MakeConnection()
    {
        Thread.Sleep(1000);

        int errorCode = await _ble.OpenDevice("Decathlon Dual HR");
        if (errorCode == 1) return false;
        
        await _ble.SetService("HeartRate");
        _ble.SubscriptionValueChanged += Controller.BleBike_SubscriptionValueChanged;
        await _ble.SubscribeToCharacteristic("HeartRateMeasurement");

        return true;
    }
    public void Reset()
    {
        _ble.Dispose();
    }
}