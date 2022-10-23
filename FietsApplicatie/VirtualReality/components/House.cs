using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class House
{

    private readonly VRClient _parent;

    private static readonly string Path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("FietsDemo", StringComparison.Ordinal)) + "VirtualReality\\resources\\";

    public House(VRClient parent)
    {
        _parent = parent;
    }

    public void PlaceHouses()
    {
        List<float[]> r = new();

        r.Add(new float[]{ 40, 80 });
        r.Add(new float[]{ -70, 20 });
        r.Add(new float[]{ -70, 0 });

        float[][] coordinates = r.ToArray();

        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<float[][], string>("positions", coordinates, 1, "scene\\terrain\\getheightterrainscene.json")!, _parent.TunnelId!)!);

        Thread.Sleep(2000);

        float[] heights = _parent.Heights;
        for (int i = 0; i < heights.Length; i++)
        {
            int type = new Random().Next(8);
            if (type == 8) type = 10;
            _parent.SendTunnel("scene/node/add", new
            {
                name = "house-" + Guid.NewGuid(),
                components = new
                {
                    transform = new
                    {
                        position = new[] { coordinates[i][0], heights[i], coordinates[i][1] },
                        scale = 6,
                        rotation = new[] { 0, 0, 0 }
                    },
                    model = new
                    {
                        file = Path + "houses\\set1\\house" + type + ".obj",
                        animated = false
                    }
                }
            });
            Thread.Sleep(10);
        }
    }
}
