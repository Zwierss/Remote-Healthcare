using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace FietsDemo;

public class Bike
{

    private readonly BLE _ble;
    public string Serial { get; set; }

    /* Creating a new BLE object. */
    public Bike()
    {
        _ble = new BLE();
    }

    /// <summary>
    /// It connects to the Tacx Flux, sets the service, subscribes to the characteristic, and returns true if it was
    /// successful
    /// </summary>
    /// <returns>
    /// The error code.
    /// </returns>
    public async Task<bool> MakeConnection()
    {
        Thread.Sleep(1000);
        
        int errorCode = await _ble.OpenDevice("Tacx Flux " + Serial);
        if (errorCode == 1) return false;
        
        errorCode = await _ble.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
        if (errorCode == 1) return false;

        _ble.SubscriptionValueChanged += HardwareConnector.BleBike_SubscriptionValueChanged;
        errorCode = await _ble.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");
        if (errorCode == 1) return false;
        
        Console.WriteLine("Connected");
        
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
    
    /// <summary>
    /// It sets the resistance of the bike.
    /// </summary>
    /// <param name="resistance">0-255</param>
    public async void SetResistance(byte resistance)
    {
        
        byte[] message = { 0xA4, 0x09, 0x4E, 0x05, 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, resistance, };
        int errorCode = await _ble.WriteCharacteristic("6e40fec3-b5a3-f393-e0a9-e50e24dcca9e", message);
    }
}