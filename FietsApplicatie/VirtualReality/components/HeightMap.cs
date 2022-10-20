using System.Drawing;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using System;

namespace VirtualReality.components;

public class HeightMap
{

    private readonly VRClient _parent;
    private static readonly string Path = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("FietsDemo", StringComparison.Ordinal)) + "VirtualReality\\resources\\";

    public HeightMap(VRClient parent)
    {
        _parent = parent;
    }

    public void RenderHeightMap()
    {
        _parent.SendData(PacketSender.GetJsonThroughTunnel<string>("scene\\resetscene.json", _parent.TunnelId!)!);
        Thread.Sleep(2000);
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>("name", "GroundPlane", 1, "scene\\node\\findnodescene.json")!, _parent.TunnelId!)!);
        Thread.Sleep(2000);
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string,string>("id", _parent.TerrainId!, 1, "scene\\node\\deletenodescene.json")!, _parent.TunnelId!)!);
        Thread.Sleep(2000);

        Console.WriteLine(Path + "heightmap-0.png");
        
        using Bitmap heightmap = new Bitmap(Path + "heightmap-0.png");
        //_parent.SendData(PacketSender.GetJsonThroughTunnel("pause.json", _parent._tunnelID));

        float[,] heightBits = new float[heightmap.Width, heightmap.Height];

        for (int x = 0; x < heightmap.Width; x++)
        {
            for (int y = 0; y < heightmap.Height; y++)
            {
                heightBits[x, y] = (heightmap.GetPixel(x, y).R / 256.0f) * 12.0f;
            }
        }
    
        Console.WriteLine(heightBits.Length);
        _parent.SendTunnel("scene/terrain/add", new
        {
            size = new[]{heightmap.Width, heightmap.Height},
            heights = heightBits.Cast<float>().ToArray()
        });
            
        _parent.SendTunnel("scene/node/add", new {
            name = "floor",
            components = new
            {
                transform = new
                {
                    position = new[] { -128, 0, -128 },
                    scale = 1
                },
                terrain = new
                {

                }
            }
        });
            
        Thread.Sleep(1000);
            
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, JObject>(
            "id", _parent.TerrainId!, 1, PacketSender.SendReplacedObject<string, JObject>(
                "diffuse", Path + "grass_green.png", 1, PacketSender.SendReplacedObject<string, string>(
                    "normal", Path + "grass_green.png", 1, "scene\\node\\addlayernodescene.json"
                )!
            )!
        )!, _parent.TunnelId)!);
    }
}