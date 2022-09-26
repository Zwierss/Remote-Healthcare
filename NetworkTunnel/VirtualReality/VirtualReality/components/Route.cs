using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Route
{

    private Client _parent;

    public Route(Client parent)
    {
        _parent = parent;
    }

    public void CreateRoute()
    {
        _parent.SendData(PacketSender.GetJsonThroughTunnel("route\\addroute.json", _parent._tunnelID));
        Thread.Sleep(1000);
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, string>(
            "route", _parent._routeID, 1, "scene\\road\\addroadscene.json"
        ), _parent._tunnelID));
        
    }
}