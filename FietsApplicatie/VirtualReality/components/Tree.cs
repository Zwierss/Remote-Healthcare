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
        //House south 1
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

        r.Add(new float[] { -41, -10 });
        r.Add(new float[] { -41, 10 });
        r.Add(new float[] { -43, -10 });
        r.Add(new float[] { -43, 10 });
        r.Add(new float[] { -45, -10 });
        r.Add(new float[] { -45, 10 });
        r.Add(new float[] { -47, -10 });
        r.Add(new float[] { -47, 10 });
        r.Add(new float[] { -49, -10 });
        r.Add(new float[] { -49, 10 });
        r.Add(new float[] { -51, -10 });
        r.Add(new float[] { -51, 10 });
        r.Add(new float[] { -53, -10 });
        r.Add(new float[] { -53, 10 });
        r.Add(new float[] { -52, 12 });
        r.Add(new float[] { -52, 14 });
        r.Add(new float[] { -52, 16 });
        r.Add(new float[] { -51, 18 });
        r.Add(new float[] { -51, 20 });
        r.Add(new float[] { -51, 22 });
        r.Add(new float[] { -51, 24 });
        r.Add(new float[] { -51, 26 });
        r.Add(new float[] { -51, 28 });
        r.Add(new float[] { -51, 30 });
        r.Add(new float[] { -51, 32 });
        r.Add(new float[] { -50, 34 });
        r.Add(new float[] { -50, 36 });
        r.Add(new float[] { -49, 38 });
        r.Add(new float[] { -49, 40 });
        r.Add(new float[] { -48, 42 });
        r.Add(new float[] { -48, 44 });
        r.Add(new float[] { -46, 46 });
        r.Add(new float[] { -46, 48 });
        r.Add(new float[] { -45, 50 });
        r.Add(new float[] { -43, 48 });
        r.Add(new float[] { -41, 46 });
        r.Add(new float[] { -40, 44 });
        r.Add(new float[] { -40, 42 });
        r.Add(new float[] { -39, 40 });
        r.Add(new float[] { -38, 38 });
        r.Add(new float[] { -36, 36 });
        r.Add(new float[] { -35, 34 });
        r.Add(new float[] { -34, 32 });
        r.Add(new float[] { -33, 30 });
        r.Add(new float[] { -32, 28 });
        r.Add(new float[] { -31, 26 });
        r.Add(new float[] { -30, 24 });
        r.Add(new float[] { -30, 22 });
        r.Add(new float[] { -29, 20 });
        r.Add(new float[] { -28, 18 });
        r.Add(new float[] { -27, 16 });
        r.Add(new float[] { -26, 14 });
        r.Add(new float[] { -25, 12 });
        r.Add(new float[] { -24, 10 });
        r.Add(new float[] { -23, 8 });
        r.Add(new float[] { -22, 6});
        r.Add(new float[] { -20, 4 });
        r.Add(new float[] { -18, 4 });
        r.Add(new float[] { -16, 4 });
        r.Add(new float[] { -14, 4 });
        r.Add(new float[] { -12, 6 });
        r.Add(new float[] { -10, 6 });
        r.Add(new float[] { -8, 8 });
        r.Add(new float[] { -6, 11 });
        r.Add(new float[] { -4, 14 });
        r.Add(new float[] { -2, 17 });
        r.Add(new float[] { 0, 20 });
        r.Add(new float[] { 2, 23 });
        r.Add(new float[] { 4, 26 });
        r.Add(new float[] { 6, 29 });
        r.Add(new float[] { 8, 32 });
        r.Add(new float[] { 10, 35 });
        r.Add(new float[] { 12, 38 });
        r.Add(new float[] { 14, 41 });
        r.Add(new float[] { 16, 44 });
        r.Add(new float[] { 18, 47 });
        r.Add(new float[] { 20, 50 });
        r.Add(new float[] { 22, 53 });
        r.Add(new float[] { 24, 56 });
        r.Add(new float[] { 26, 59 });
        r.Add(new float[] { 28, 56 });
        r.Add(new float[] { 30, 54 });
        r.Add(new float[] { 32, 52 });
        r.Add(new float[] { 34, 50 });
        r.Add(new float[] { 36, 49 });
        r.Add(new float[] { 38, 49 });
        r.Add(new float[] { 40, 50 });
        r.Add(new float[] { 42, 51 });
        r.Add(new float[] { 44, 52 });
        r.Add(new float[] { 45, 50 });
        r.Add(new float[] { 46, 48 });
        r.Add(new float[] { 47, 46 });
        r.Add(new float[] { 45, 45 });
        r.Add(new float[] { 43, 44 });
        r.Add(new float[] { 41, 43 });
        r.Add(new float[] { 39, 43 });
        r.Add(new float[] { 37, 42 });
        r.Add(new float[] { 35, 40 });
        r.Add(new float[] { 34, 38 });
        r.Add(new float[] { 34, 36 });
        r.Add(new float[] { 34, 34 });
        r.Add(new float[] { 35, 32 });
        r.Add(new float[] { 36, 30 });
        r.Add(new float[] { 37, 28 });
        r.Add(new float[] { 38, 26 });

        r.Add(new float[] { -63, -12 });
        r.Add(new float[] { -65, -12 });
        r.Add(new float[] { -67, -12 });
        r.Add(new float[] { -69, -12 });
        r.Add(new float[] { -71, -12 });

        //House south 2
        r.Add(new float[] { -59, 40 });
        r.Add(new float[] { -61, 39 });
        r.Add(new float[] { -59, 38 });
        r.Add(new float[] { -61, 37 });
        r.Add(new float[] { -59, 36 });
        r.Add(new float[] { -61, 35 });
        r.Add(new float[] { -59, 34 });
        r.Add(new float[] { -61, 33 });
        r.Add(new float[] { -59, 32 });
        r.Add(new float[] { -61, 31 });
        r.Add(new float[] { -59, 30 });
        r.Add(new float[] { -61, 30 });
        r.Add(new float[] { -63, 30 });
        r.Add(new float[] { -65, 30 });
        r.Add(new float[] { -67, 30 });
        r.Add(new float[] { -69, 30 });
        r.Add(new float[] { -71, 30 });

        //Sharp corner south-east
        r.Add(new float[] { -55, 50 });
        r.Add(new float[] { -55, 51 });
        r.Add(new float[] { -54, 52 });
        r.Add(new float[] { -55, 52 });
        r.Add(new float[] { -56, 52 });
        r.Add(new float[] { -57, 52 });
        r.Add(new float[] { -58, 52 });
        r.Add(new float[] { -59, 52 });
        r.Add(new float[] { -60, 52 });
        r.Add(new float[] { -54, 53 });
        r.Add(new float[] { -53, 54 });
        r.Add(new float[] { -53, 55 });
        r.Add(new float[] { -50, 56 });
        r.Add(new float[] { -48, 57 });
        r.Add(new float[] { -47, 58 });
        r.Add(new float[] { -45, 58 });
        r.Add(new float[] { -44, 58 });
        r.Add(new float[] { -42, 56 });
        r.Add(new float[] { -41, 56 });
        r.Add(new float[] { -39, 54 });
        r.Add(new float[] { -38, 54 });
        r.Add(new float[] { -36, 52 });
        r.Add(new float[] { -35, 52 });
        r.Add(new float[] { -33, 50 });
        r.Add(new float[] { -33, 48 });
        

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
        r.Add(new float[] { -61, -22 });
        r.Add(new float[] { -63, -23 });
        r.Add(new float[] { -60, -24 });
        r.Add(new float[] { -62, -25 });
        r.Add(new float[] { -60, -26 });
        r.Add(new float[] { -62, -27 });
        r.Add(new float[] { -60, -28 });
        r.Add(new float[] { -62, -29 });
        r.Add(new float[] { -60, -30 });
        r.Add(new float[] { -62, -31 });
        r.Add(new float[] { -60, -32 });
        r.Add(new float[] { -62, -33 });
        r.Add(new float[] { -60, -34 });
        r.Add(new float[] { -62, -35 });
        r.Add(new float[] { -60, -36 });
        r.Add(new float[] { -62, -37 });
        r.Add(new float[] { -59, -38 });
        r.Add(new float[] { -61, -39 });
        r.Add(new float[] { -59, -40 });
        r.Add(new float[] { -61, -41 });
        r.Add(new float[] { -58, -42 });
        r.Add(new float[] { -60, -43 });
        r.Add(new float[] { -58, -44 });
        r.Add(new float[] { -60, -45 });



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