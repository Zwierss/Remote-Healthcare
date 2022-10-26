using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Panel
{

    private VRClient _parent;
    
    public Panel(VRClient parent)
    {
        _parent = parent;
    }

    public void AddPanel()
    {
        _parent.SendTunnel("scene/node/add", new
        {
            name = "Panel",
            parent = _parent.HeadId,
            components = new
            {
                transform = new
                {
                   position = new[]{0,0.1, -1.6},
                   scale = 1,
                   rotation = new[]{0, 0, 0}
                },
                panel = new
                {
                    size = new[]{0.5,0.5},
                    resolution = new[]{512,200},
                    castShadow = false
                }
            }
        });
        
        Thread.Sleep(1000);
    }

    public void UpdatePanel(double speed, double heartRate, string message)
    {
        string tunnelId = _parent.TunnelId!;
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, string>(
            "id", _parent.PanelId!, 1, "scene\\panel\\clearpanelscene.json"
        )!,tunnelId)!);
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,JObject>(
            "id", _parent.PanelId!, 1, PacketSender.SendReplacedObject<string,string>(
                "text" ,speed + " km/h", 1, "scene\\panel\\drawtextpanelscene.json"
            )!
        )!,tunnelId)!);
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,JObject>(
            "id", _parent.PanelId!, 1, PacketSender.SendReplacedObject<string,JObject>(
                "text" ,heartRate + " bpm", 1, PacketSender.SendReplacedObject<int[],string>(
                    "position", new[]{50,70}, 1, "scene\\panel\\drawtextpanelscene.json"
                )!
            )!
        )!,tunnelId)!);
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,JObject>(
            "id", _parent.PanelId!, 1, PacketSender.SendReplacedObject<string,JObject>(
                "text" ,message, 1, PacketSender.SendReplacedObject<int[],string>(
                    "position", new[]{50,100}, 1, "scene\\panel\\drawtextpanelscene.json"
                )!
            )!
        )!,tunnelId)!);
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>(
            "id", _parent.PanelId!, 1, "scene\\panel\\setclearcolorpanelscene.json"
        )!, tunnelId)!);
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>(
            "id", _parent.PanelId!, 1, "scene\\panel\\swappanelscene.json"
        )!, tunnelId)!);
    }
}