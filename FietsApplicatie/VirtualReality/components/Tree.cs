﻿using Newtonsoft.Json.Linq;

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