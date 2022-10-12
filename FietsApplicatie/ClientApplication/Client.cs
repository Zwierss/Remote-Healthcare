using System.Net.Sockets;
using FietsDemo;
using VirtualReality;

namespace ClientApplication;

public class Client : IClientCallback
{
    private VRClient _vr;
    

    public Client()
    {
        _vr = new VRClient();
        HardwareConnector.SetupHardware(this);
    }


    public void OnNewBikeData(IReadOnlyList<int> values)
    {
        double speed = ((values[8] + values[9] * 255) * 0.001) * 3.6;
        _vr.UpdateBikeSpeed(speed);
    }

    public void OnNewHeartrateData(IReadOnlyList<int> values)
    {
        _vr.UpdatePanel(values[1]);
    }
}