using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Camera
{

    private readonly Client _parent;

    public Camera(Client parent)
    {
        _parent = parent;
    }

    public void SetCamera()
    {
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>("name", "camera", 1, "scene\\node\\findnodescene.json")!, _parent.TunnelId!)!);
    }
}