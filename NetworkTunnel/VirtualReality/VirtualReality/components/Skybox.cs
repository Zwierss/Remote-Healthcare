using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Skybox
{
    private Client _parent;
    private double _count;
    
    public Skybox(Client parent)
    {
        _parent = parent;
        _count = 24.0;
    }

    public void update()
    {
        while (true)
        {
            if (_count >= 24.0)
            {
                _count = 0.0;
            }

            _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<double, string>("time", _count, 1, "scene\\skybox\\settimeskyboxscene.json")!, _parent._tunnelID!)!);
            _count += 0.05;
            Thread.Sleep(500);
            
        }
    }
}