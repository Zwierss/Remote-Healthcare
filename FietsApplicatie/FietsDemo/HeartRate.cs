using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo;

public class HeartRate
{

    private readonly BLE _ble;

    /* Creating a new instance of the BLE class. */
    public HeartRate()
    {
        _ble = new BLE();
    }

    /// <summary>
    /// It opens a connection to the device, sets the service to the HeartRate service, subscribes to the
    /// HeartRateMeasurement characteristic, and returns true if the connection was successful
    /// </summary>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public async Task<bool> MakeConnection()
    {
        Thread.Sleep(1000);

        int errorCode = await _ble.OpenDevice("Decathlon Dual HR");
        if (errorCode == 1) return false;
        
        await _ble.SetService("HeartRate");
        _ble.SubscriptionValueChanged += HardwareConnector.BleBike_SubscriptionValueChanged;
        await _ble.SubscribeToCharacteristic("HeartRateMeasurement");

        return true;
    }

    /// <summary>
    /// It disconnects the device from the BLE.
    /// </summary>
    public void Disconnect()
    {
        _ble.CloseDevice();
        _ble.Dispose();
    }
}