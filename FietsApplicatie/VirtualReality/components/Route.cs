using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Route
{

    private readonly VRClient _parent;

    /* A constructor. */
    public Route(VRClient parent)
    {
        _parent = parent;
    }

    /// <summary>
    /// It sends a request to the server to create a route, waits for a second, then sends a request to create a road scene
    /// </summary>
    public void CreateRoute()
    {
        _parent.SendData(PacketSender.GetJsonThroughTunnel<string>("route\\addroute.json", _parent.TunnelId!)!);
        Thread.Sleep(1000);
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, string>(
            "route", _parent.RouteId!, 1, "scene\\road\\addroadscene.json"
        )!, _parent.TunnelId!)!);
        
    }
}