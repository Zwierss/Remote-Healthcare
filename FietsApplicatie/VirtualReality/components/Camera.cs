using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Camera
{

    private readonly VRClient _parent;

    public Camera(VRClient parent)
    {
        _parent = parent;
    }

    public void SetCamera()
    {
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>("name", "Camera", 1, "scene\\node\\findnodescene.json")!, _parent.TunnelId!)!);
        Thread.Sleep(2000);

        _parent.SendData(PacketSender.GetJsonThroughTunnel(PacketSender.SendReplacedObject<string,JObject>(
            "id", _parent.CameraId!, 1, PacketSender.SendReplacedObject<string,string>(
                "parent", _parent.BikeId!, 1, "scene\\node\\updatenodescene.json"
            )!
        ), _parent.TunnelId!)!);
    }
}