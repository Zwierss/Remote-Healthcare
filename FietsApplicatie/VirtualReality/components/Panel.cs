using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Panel
{

    private VRClient _parent;
    
    /* The constructor of the class. It is called when the class is instantiated. */
    public Panel(VRClient parent)
    {
        _parent = parent;
    }

    /// <summary>
    /// > Add a panel to the scene
    /// </summary>
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
                    size = new[]{0.2,0.2},
                    resolution = new[]{512,200},
                    castShadow = false
                }
            }
        });
        
        Thread.Sleep(1000);
    }

    /// <summary>
    /// It clears the panel, draws the speed, heart rate and message, sets the clear color to black and swaps the panel
    /// </summary>
    /// <param name="speed">The speed of the player in km/h</param>
    /// <param name="heartRate">The heart rate of the user.</param>
    /// <param name="message">The message to be displayed on the panel.</param>
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
                    "position", new[]{50,80}, 1, "scene\\panel\\drawtextpanelscene.json"
                )!
            )!
        )!,tunnelId)!);

        if (!string.IsNullOrEmpty(message))
        {
            string[] uncoveredMessage = message.Split(' ');
            int newLines = 0;
            string newMessage = "";

            foreach (string s in uncoveredMessage)
            {
                newMessage += s + " ";
                if (newMessage.Length + s.Length > 22 || s == uncoveredMessage[uncoveredMessage.Length - 1]) 
                {
                    int pos = 160 + newLines * 40;
                    _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, JObject>(
                        "id", _parent.PanelId!, 1, PacketSender.SendReplacedObject<string, JObject>(
                            "text", newMessage, 1, PacketSender.SendReplacedObject<int[], string>(
                                "position", new[] { 50, pos }, 1, "scene\\panel\\drawtextpanelscene.json"
                            )!
                        )!
                    )!, tunnelId)!);
                    newLines++;
                    newMessage = "";
                }
            }
        }
        else 
        {
            _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, JObject>(
                "id", _parent.PanelId!, 1, PacketSender.SendReplacedObject<string, JObject>(
                    "text", message, 1, PacketSender.SendReplacedObject<int[], string>(
                        "position", new[] { 50, 160 }, 1, "scene\\panel\\drawtextpanelscene.json"
                    )!
                )!
            )!, tunnelId)!);
        }
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>(
            "id", _parent.PanelId!, 1, "scene\\panel\\setclearcolorpanelscene.json"
        )!, tunnelId)!);
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>(
            "id", _parent.PanelId!, 1, "scene\\panel\\swappanelscene.json"
        )!, tunnelId)!);
    }
}