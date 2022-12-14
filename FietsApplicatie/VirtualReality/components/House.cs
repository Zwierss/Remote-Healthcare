using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class House
{

    private readonly VRClient _parent;

    private static readonly string Path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("ClientGUI", StringComparison.Ordinal)) + "VirtualReality\\resources\\";

    /* A constructor. */
    public House(VRClient parent)
    {
        _parent = parent;
    }

    /// <summary>
    /// It takes a list of coordinates, sends them to the server, waits for the server to send back the heights of the
    /// terrain at those coordinates, then places a random house at each coordinate
    /// </summary>
    public void PlaceHouses()
    {
        List<float[]> r = new();

        r.Add(new float[]{ 40, 80 });
        r.Add(new float[]{ -68, 45 });
        r.Add(new float[]{ -68, 20 });
        r.Add(new float[]{ -45, 0 });
        r.Add(new float[] { -45, -25 });
        r.Add(new float[]{ -70, 0 });
        r.Add(new float[]{ 12, -10 });
        r.Add(new float[]{ 22, -20 });
        r.Add(new float[]{ 15, -52 });
        r.Add(new float[]{ 35, -70 });

        float[][] coordinates = r.ToArray();

        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<float[][], string>("positions", coordinates, 1, "scene\\terrain\\getheightterrainscene.json")!, _parent.TunnelId!)!);

        Thread.Sleep(2000);

        float[] heights = _parent.Heights;
        try
        {
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
        catch (Exception)
        {
            Console.WriteLine("error wile rendering houses");
        }
    }
}
