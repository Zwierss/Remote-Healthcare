using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Tree
{

    private readonly Client _parent;
    
    private static readonly string BikePath = string.Concat(Environment.CurrentDirectory.AsSpan(0, Environment.CurrentDirectory.LastIndexOf("bin", StringComparison.Ordinal)), "resources\\");
    
    public Tree(Client parent)
    {
        _parent = parent;
    }

    public void PlaceTrees()
    {
        List<float[]> r = new();
        int quantity = 50;
        
        for (int i = 0; i < quantity; i++)
        {
            float x = new Random().Next(200) - 100;
            float z = new Random().Next(200) - 100;
            
            r.Add(new[]{x,z});
        }

        float[][] coordinates = r.ToArray();
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<float[][], string>("positions", coordinates, 1, "scene\\terrain\\getheightterrainscene.json")!, _parent.TunnelId!)!);

        Thread.Sleep(2000);

        float[] heights = _parent.Heights;
        for (int i = 0; i < heights.Length; i++)
        {
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
                        file = BikePath + "trees\\fantasy\\tree1.obj",
                        animated = false
                    }
                }
            });   
        }
    }
}