using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Tree
{

    private readonly VRClient _parent;
    
    private static readonly string Path = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("FietsDemo", StringComparison.Ordinal)) + "VirtualReality\\resources\\";
    
    public Tree(VRClient parent)
    {
        _parent = parent;
    }

    public void PlaceTrees()
    {
        List<float[]> r = new();
        //House south
        r.Add(new float[] { -62, -10 });
        r.Add(new float[] { -62, 10 });
        r.Add(new float[] { -64, -10 });
        r.Add(new float[] { -64, 10 });
        r.Add(new float[] { -66, -10 });
        r.Add(new float[] { -66, 10 });
        r.Add(new float[] { -68, -10 });
        r.Add(new float[] { -68, 10 });
        r.Add(new float[] { -70, -10 });
        r.Add(new float[] { -70, 10 });
        r.Add(new float[] { -74, 10 });
        r.Add(new float[] { -72, 10 });
        r.Add(new float[] { -74, -10 });
        r.Add(new float[] { -74, 10 });
        r.Add(new float[] { -63, -12 });
        r.Add(new float[] { -65, -12 });
        r.Add(new float[] { -67, -12 });
        r.Add(new float[] { -69, -12 });
        r.Add(new float[] { -71, -12 });

        r.Add(new float[] { -62, -12 });
        r.Add(new float[] { -64, -13 });
        r.Add(new float[] { -62, -14 });
        r.Add(new float[] { -64, -15 });
        r.Add(new float[] { -61, -16 });
        r.Add(new float[] { -63, -17 });
        r.Add(new float[] { -61, -18 });
        r.Add(new float[] { -63, -19 });
        r.Add(new float[] { -61, -20 });
        r.Add(new float[] { -63, -21 });
        


        float[][] coordinates = r.ToArray();
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<float[][], string>("positions", coordinates, 1, "scene\\terrain\\getheightterrainscene.json")!, _parent.TunnelId!)!);

        Thread.Sleep(2000);

        float[] heights = _parent.Heights;
        for (int i = 0; i < heights.Length; i++)
        {
            int type = new Random().Next(9) + 1;
            
            _parent.SendTunnel("scene/node/add", new
            {
                name = "tree-" + Guid.NewGuid(),
                components = new
                {
                    transform = new
                    {
                        position = new[]{coordinates[i][0],heights[i],coordinates[i][1]},
                        scale = 1,
                        rotation = new[]{0,0,0}
                    },
                    model = new
                    {
                        file = Path + "trees\\fantasy\\tree" + type + ".obj",
                        animated = false
                    }
                }
            });
            Thread.Sleep(10);
        }
    }
}