using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Camera
{

    private readonly VRClient _parent;

    /* A constructor. */
    public Camera(VRClient parent)
    {
        _parent = parent;
    }

    /// <summary>
    /// > We find the camera and head nodes, then we update the camera node's parent to be the bike node
    /// </summary>
    public void SetView()
    {
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>("name", "Camera", 1, "scene\\node\\findnodescene.json")!, _parent.TunnelId!)!);
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>("name", "Head", 1, "scene\\node\\findnodescene.json")!, _parent.TunnelId!)!);
        Thread.Sleep(2000);

        _parent.SendData(PacketSender.GetJsonThroughTunnel(PacketSender.SendReplacedObject<string,JObject>(
            "id", _parent.CameraId!, 1, PacketSender.SendReplacedObject<string,string>(
                "parent", _parent.BikeId!, 1, "scene\\node\\updatenodescene.json"
            )!
        ), _parent.TunnelId!)!);
    }
}