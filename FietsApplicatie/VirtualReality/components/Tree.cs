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
        
        float x = 40;
        float z = 40;
        r.Add(new[]{x,z});
        

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