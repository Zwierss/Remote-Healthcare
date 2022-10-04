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
        _parent.SendData(PacketSender.GetJsonThroughTunnel(PacketSender.SendReplacedObject(
            "name", "Head", 1, "scene\\node\\findnodescene.json"
        ), _parent.TunnelId!)!);
        Thread.Sleep(1000);
        
        _parent.SendTunnel("scene/node/add", new
        {
            name = "Panel",
            parent = _parent.HeadId,
            components = new
            {
                transform = new
                {
                   position = new[]{0,0, 2},
                   scale = 1,
                   rotation = new[]{0, 0, 0}
                },
                panel = new
                {
                    size = new[]{1,1},
                    resolution = new[]{512,512},
                }
            }
        });
        
        Thread.Sleep(1000);
        
        UpdateSpeed(0.0);
    }

    public void UpdateSpeed(double speed)
    {
        _parent.SendData(PacketSender.GetJsonThroughTunnel(PacketSender.SendReplacedObject(
            "id", _parent.PanelId, 1, PacketSender.SendReplacedObject(
                "text" ,speed + " km/h", 1, "scene\\panel\\drawtextpanelscene.json"
            )
        ),_parent.TunnelId!)!);
        _parent.SendData(PacketSender.GetJsonThroughTunnel(PacketSender.SendReplacedObject(
            "id", _parent.PanelId, 1, "scene\\panel\\swappanelscene.json"
        ), _parent.TunnelId!)!);
    }
}