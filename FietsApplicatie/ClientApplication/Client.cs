using System.Net.Sockets;
using FietsDemo;
using VirtualReality;
using static VirtualReality.PacketSender;

namespace ClientApplication;

public class Client : IClientCallback
{
    private readonly VRClient _vr;
    private readonly TcpClient _client;
    

    public Client()
    {
        _vr = new VRClient();
    }

    public async Task SetupConnection()
    {
        HardwareConnector.SetupHardware(this);
        await _vr.StartConnection();
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