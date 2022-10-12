using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Bike
{

    private readonly VRClient _parent;
    
    private static readonly string BikePath = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("ClientApplication", StringComparison.Ordinal)) + "VirtualReality\\resources\\";
    public Bike(VRClient parent)
    {
        _parent = parent;
    }

    public void PlaceBike()
    {

        Console.WriteLine("placing bike");
        _parent.SendTunnel("scene/node/add", new {
            name = "bike",
            components = new
            {
                transform = new
                {
                    position = new[] { 0, 0.15, 0 },
                    scale = 0.01
                },
                model = new
                {
                    file = BikePath + "bike\\bike_anim.fbx",
                    animated = true,
                    animation = "Armature|Fietsen"
                }
            }
        });

        Thread.Sleep(1000);
        
        Console.WriteLine("setting route");
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, JObject>(
            "route", _parent.RouteId!, 1, PacketSender.SendReplacedObject<string,JObject>(
                "node", _parent.BikeId!, 1, PacketSender.SendReplacedObject<double,string>(
                    "speed", 10.0, 1, "route\\followroute.json")!)!
        )!, _parent.TunnelId!)!);
        Console.WriteLine("done with bike");
    }
}