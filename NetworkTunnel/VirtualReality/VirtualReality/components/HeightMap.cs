using System.Drawing;
using System.Net.Mime;
using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class HeightMap
{

    private Client _parent;
    private static string _PATH = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin", StringComparison.Ordinal)) + "resources\\";

    public HeightMap(Client parent)
    {
        _parent = parent;
    }

    public void RenderHeightMap()
    {
        using (Bitmap heightmap = new Bitmap(_PATH + "heightmap.png"))
        {
            _parent.SendData(PacketSender.GetJsonThroughTunnel<string>("scene\\resetscene.json", _parent._tunnelID));
            //_parent.SendData(PacketSender.GetJsonThroughTunnel("pause.json", _parent._tunnelID));

            float[,] heightBits = new float[heightmap.Width, heightmap.Height];

            for (int x = 0; x < heightmap.Width; x++)
            {
                for (int y = 0; y < heightmap.Height; y++)
                {
                    heightBits[x, y] = (heightmap.GetPixel(x, y).R / 256.0f) * 25.0f;
                }
            }
    
            Console.WriteLine(heightBits.Length);
            _parent.sendTunnel("scene/terrain/add", new
            {
                size = new[]{heightmap.Width, heightmap.Height},
                heights = heightBits.Cast<float>().ToArray()
            });
            
            _parent.sendTunnel("scene/node/add", new {
                name = "floor",
                components = new
                {
                    transform = new
                    {
                        position = new[] { -128, -14, -128 },
                        scale = 1
                    },
                    terrain = new
                    {

                    }
                }
            });
            
            Thread.Sleep(1000);
            
            _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<string, JObject>(
                "id", _parent._nodeID!, 1, PacketSender.SendReplacedObject<string, JObject>(
                    "diffuse", _PATH + "grass_normal.png", 1, PacketSender.SendReplacedObject<string, string>(
                        "normal", _PATH + "grass_normal.png", 1, "scene\\node\\addlayernodescene.json"
                    )!
                )!
            )!, _parent._tunnelID)!);
        }
    }
}