using Newtonsoft.Json.Linq;

namespace VirtualReality.components;

public class Tree
{

    private readonly VRClient _parent;
    
    private static readonly string Path = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("ClientGUI", StringComparison.Ordinal)) + "VirtualReality\\resources\\";
    
    /* Creating a constructor for the Tree class. */
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
        r.Add(new float[] { 37, 24 });
        r.Add(new float[] { 36, 22 });
        r.Add(new float[] { 34, 21 });
        r.Add(new float[] { 32, 20 });
        r.Add(new float[] { 30, 20 });
        r.Add(new float[] { 28, 19 });
        r.Add(new float[] { 26, 18 });
        r.Add(new float[] { 24, 17 });
        r.Add(new float[] { 22, 16 });
        r.Add(new float[] { 20, 15 });
        r.Add(new float[] { 18, 13 });
        r.Add(new float[] { 16, 10 });
        r.Add(new float[] { 14, 7 });
        r.Add(new float[] { 14, 5 });
        r.Add(new float[] { 15, 3 });
        r.Add(new float[] { 15, 1 });
        r.Add(new float[] { 16, 0 });
        r.Add(new float[] { 16, -2 });
        r.Add(new float[] { 17, -4 });
        r.Add(new float[] { 15, -4 });
        r.Add(new float[] { 13, -4 });
        r.Add(new float[] { 11, -4 });
        r.Add(new float[] { 9, -4 });
        r.Add(new float[] { 7, -4 });
        r.Add(new float[] { 5, -4 });
        r.Add(new float[] { 29, -17 });
        r.Add(new float[] { 27, -17 });
        r.Add(new float[] { 25, -17 });
        r.Add(new float[] { 23, -17 });
        r.Add(new float[] { 21, -17 });
        r.Add(new float[] { 19, -17 });
        r.Add(new float[] { 17, -17 });
        r.Add(new float[] { 15, -17 });
        r.Add(new float[] { 35, -25 });
        r.Add(new float[] { 33, -25 });
        r.Add(new float[] { 31, -25 });
        r.Add(new float[] { 29, -25 });
        r.Add(new float[] { 27, -25 });
        r.Add(new float[] { 25, -25 });
        r.Add(new float[] { 23, -25 });
        r.Add(new float[] { 21, -25 });
        r.Add(new float[] { 37, -27 });
        r.Add(new float[] { 39, -29 });
        r.Add(new float[] { 41, -31 });
        r.Add(new float[] { 43, -33 });
        r.Add(new float[] { 45, -35 });
        r.Add(new float[] { 47, -37 });
        r.Add(new float[] { 47, -39 });
        r.Add(new float[] { 47, -41 });
        r.Add(new float[] { 47, -43 });
        r.Add(new float[] { 47, -45 });
        r.Add(new float[] { 47, -47 });
        r.Add(new float[] { 45, -49 });
        r.Add(new float[] { 43, -51 });
        r.Add(new float[] { 41, -53 });
        r.Add(new float[] { 39, -54 });
        r.Add(new float[] { 37, -55 });
        r.Add(new float[] { 35, -56 });
        r.Add(new float[] { 33, -57 });
        r.Add(new float[] { 31, -58 });
        r.Add(new float[] { 29, -58 });
        r.Add(new float[] { 27, -58 });
        r.Add(new float[] { 25, -58 });
        r.Add(new float[] { 23, -56 });
        r.Add(new float[] { 23, -54 });
        r.Add(new float[] { 23, -52 });
        r.Add(new float[] { 23, -50 });
        r.Add(new float[] { 23, -48 });
        r.Add(new float[] { 23, -46 });
        r.Add(new float[] { 23, -44 });
        r.Add(new float[] { 12, -60 });
        r.Add(new float[] { 10, -58 });
        r.Add(new float[] { 10, -56 });
        r.Add(new float[] { 10, -54 });
        r.Add(new float[] { 10, -52 });
        r.Add(new float[] { 10, -50 });
        r.Add(new float[] { 10, -61 });
        r.Add(new float[] { 8, -62 });
        r.Add(new float[] { 6, -63 });
        r.Add(new float[] { 4, -64 });
        r.Add(new float[] { 2, -66 });
        r.Add(new float[] { 0, -68 });
        r.Add(new float[] { -2, -70 });
        r.Add(new float[] { -4, -73 });
        r.Add(new float[] { -6, -76 });
        r.Add(new float[] { -8, -79 });
        r.Add(new float[] { -10, -82 });
        r.Add(new float[] { -12, -84 });
        r.Add(new float[] { -14, -82 });
        r.Add(new float[] { -16, -80 });
        r.Add(new float[] { -18, -78 });
        r.Add(new float[] { -20, -76 });
        r.Add(new float[] { -22, -73 });
        r.Add(new float[] { -24, -70 });
        r.Add(new float[] { -26, -67 });
        r.Add(new float[] { -28, -64 });
        r.Add(new float[] { -30, -62 });
        r.Add(new float[] { -32, -60 });
        r.Add(new float[] { -34, -59 });
        r.Add(new float[] { -36, -58 });
        r.Add(new float[] { -38, -56 });
        r.Add(new float[] { -40, -54 });
        r.Add(new float[] { -42, -52 });
        r.Add(new float[] { -44, -50 });
        r.Add(new float[] { -46, -48 });
        r.Add(new float[] { -48, -45 });
        r.Add(new float[] { -49, -42 });
        r.Add(new float[] { -50, -39 });
        r.Add(new float[] { -50, -37 });
        r.Add(new float[] { -50, -35 });
        r.Add(new float[] { -50, -33 });
        r.Add(new float[] { -47, -33 });
        r.Add(new float[] { -45, -33 });
        r.Add(new float[] { -43, -33 });
        r.Add(new float[] { -41, -33 });
        r.Add(new float[] { -39, -33 });
        r.Add(new float[] { -53, -20 });
        r.Add(new float[] { -53, -18 });
        r.Add(new float[] { -53, -16 });
        r.Add(new float[] { -53, -14 });
        r.Add(new float[] { -53, -12 });

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
        r.Add(new float[] { -56, -46 });
        r.Add(new float[] { -55, -48 });
        r.Add(new float[] { -54, -50 });
        r.Add(new float[] { -53, -52 });
        r.Add(new float[] { -52, -54 });
        r.Add(new float[] { -51, -56 });
        r.Add(new float[] { -50, -58 });
        r.Add(new float[] { -48, -60 });
        r.Add(new float[] { -46, -62 });
        r.Add(new float[] { -44, -64 });
        r.Add(new float[] { -41, -66 });
        r.Add(new float[] { -38, -68 });
        r.Add(new float[] { -35, -70 });
        r.Add(new float[] { -32, -72 });
        r.Add(new float[] { -30, -74 });
        r.Add(new float[] { -29, -76 });
        r.Add(new float[] { -28, -78 });
        r.Add(new float[] { -27, -80 });
        r.Add(new float[] { -26, -82 });
        r.Add(new float[] { -24, -84 });
        r.Add(new float[] { -22, -86 });
        r.Add(new float[] { -20, -88 });
        r.Add(new float[] { -18, -90 });
        r.Add(new float[] { -16, -91 });
        r.Add(new float[] { -14, -92 });
        r.Add(new float[] { -12, -92 });
        r.Add(new float[] { -10, -91 });
        r.Add(new float[] { -8, -90 });
        r.Add(new float[] { -6, -89 });
        r.Add(new float[] { -4, -88 });
        r.Add(new float[] { -2, -86 });
        r.Add(new float[] { 0, -84 });
        r.Add(new float[] { 2, -81 });
        r.Add(new float[] { 4, -78 });
        r.Add(new float[] { 6, -76 });
        r.Add(new float[] { 8, -74 });
        r.Add(new float[] { 10, -72 });
        r.Add(new float[] { 12, -70 });
        r.Add(new float[] { 14, -69 });
        r.Add(new float[] { 16, -68 });
        r.Add(new float[] { 18, -68 });
        r.Add(new float[] { 20, -68 });
        r.Add(new float[] { 22, -67 });
        r.Add(new float[] { 24, -66 });
        r.Add(new float[] { 26, -66 });
        r.Add(new float[] { 28, -66 });
        r.Add(new float[] { 30, -66 });
        r.Add(new float[] { 42, -62 });
        r.Add(new float[] { 44, -61 });
        r.Add(new float[] { 46, -60 });
        r.Add(new float[] { 48, -58 });
        r.Add(new float[] { 50, -56 });
        r.Add(new float[] { 52, -54 });
        r.Add(new float[] { 54, -52 });
        r.Add(new float[] { 55, -50 });
        r.Add(new float[] { 56, -48 });
        r.Add(new float[] { 56, -46 });
        r.Add(new float[] { 56, -44 });
        r.Add(new float[] { 56, -42 });
        r.Add(new float[] { 56, -40 });
        r.Add(new float[] { 55, -38 });
        r.Add(new float[] { 55, -36 });
        r.Add(new float[] { 54, -34 });
        r.Add(new float[] { 54, -32 });
        r.Add(new float[] { 52, -30 });
        r.Add(new float[] { 50, -28 });
        r.Add(new float[] { 49, -26 });
        r.Add(new float[] { 48, -24 });
        r.Add(new float[] { 46, -22 });
        r.Add(new float[] { 44, -20 });
        r.Add(new float[] { 42, -18 });
        r.Add(new float[] { 40, -16 });
        r.Add(new float[] { 38, -14 });
        r.Add(new float[] { 36, -12 });
        r.Add(new float[] { 34, -10 });
        r.Add(new float[] { 32, -8 });
        r.Add(new float[] { 30, -6 });
        r.Add(new float[] { 28, -4 });
        r.Add(new float[] { 26, -2 });
        r.Add(new float[] { 24, 0 });
        r.Add(new float[] { 22, 2 });
        r.Add(new float[] { 22, 4 });
        r.Add(new float[] { 24, 6 });
        r.Add(new float[] { 26, 8 });
        r.Add(new float[] { 28, 9 });
        r.Add(new float[] { 30, 10 });
        r.Add(new float[] { 32, 11 });
        r.Add(new float[] { 34, 12 });
        r.Add(new float[] { 36, 13 });
        r.Add(new float[] { 38, 14 });
        r.Add(new float[] { 40, 15 });
        r.Add(new float[] { 42, 16 });
        r.Add(new float[] { 44, 19 });
        r.Add(new float[] { 46, 22 });
        r.Add(new float[] { 46, 24 });
        r.Add(new float[] { 46, 26 });
        r.Add(new float[] { 46, 28 });
        r.Add(new float[] { 45, 30 });
        r.Add(new float[] { 45, 32 });
        r.Add(new float[] { 44, 34 });
        r.Add(new float[] { 44, 36 });
        r.Add(new float[] { 46, 36 });
        r.Add(new float[] { 48, 36 });
        r.Add(new float[] { 50, 36 });
        r.Add(new float[] { 52, 37 });
        r.Add(new float[] { 54, 38 });
        r.Add(new float[] { 56, 41 });
        r.Add(new float[] { 56, 44 });
        r.Add(new float[] { 56, 46 });
        r.Add(new float[] { 56, 48 });
        r.Add(new float[] { 55, 50 });
        r.Add(new float[] { 54, 52 });
        r.Add(new float[] { 53, 54 });
        r.Add(new float[] { 52, 56 });
        r.Add(new float[] { 50, 58 });
        r.Add(new float[] { 48, 60 });
        r.Add(new float[] { 45, 61 });
        r.Add(new float[] { 42, 60 });
        r.Add(new float[] { 40, 60 });
        r.Add(new float[] { 38, 59 });
        r.Add(new float[] { 36, 59 });
        r.Add(new float[] { 34, 62 });
        r.Add(new float[] { 32, 64 });
        r.Add(new float[] { 30, 66 });
        r.Add(new float[] { 28, 66 });
        r.Add(new float[] { 26, 66 });
        r.Add(new float[] { 24, 66 });
        r.Add(new float[] { 22, 64 });
        r.Add(new float[] { 20, 62 });
        r.Add(new float[] { 18, 60 });
        r.Add(new float[] { 16, 58 });
        r.Add(new float[] { 14, 56 });
        r.Add(new float[] { 12, 54 });
        r.Add(new float[] { 10, 51 });
        r.Add(new float[] { 8, 48 });
        r.Add(new float[] { 6, 45 });
        r.Add(new float[] { 4, 42 });
        r.Add(new float[] { 2, 39 });
        r.Add(new float[] { 0, 36 });
        r.Add(new float[] { -2, 33 });
        r.Add(new float[] { -4, 30 });
        r.Add(new float[] { -6, 27 });
        r.Add(new float[] { -8, 24 });
        r.Add(new float[] { -10, 21 });
        r.Add(new float[] { -12, 18 });
        r.Add(new float[] { -14, 15 });
        r.Add(new float[] { -16, 12 });
        r.Add(new float[] { -18, 15 });
        r.Add(new float[] { -19, 17 });
        r.Add(new float[] { -20, 19 });
        r.Add(new float[] { -21, 21 });
        r.Add(new float[] { -22, 23 });
        r.Add(new float[] { -23, 25 });
        r.Add(new float[] { -24, 27 });
        r.Add(new float[] { -25, 29 });
        r.Add(new float[] { -26, 31 });
        r.Add(new float[] { -27, 33 });
        r.Add(new float[] { -28, 35 });
        r.Add(new float[] { -29, 37 });
        r.Add(new float[] { -30, 39 });
        r.Add(new float[] { -31, 41 });
        r.Add(new float[] { -32, 43 });
        r.Add(new float[] { -33, 45 });
        r.Add(new float[] { -34, 47 });

        float[][] coordinates = r.ToArray();
        
        _parent.SendData(PacketSender.GetJsonThroughTunnel<JObject>(PacketSender.SendReplacedObject<float[][], string>("positions", coordinates, 1, "scene\\terrain\\getheightterrainscene.json")!, _parent.TunnelId!)!);

        Thread.Sleep(2000);

        float[] heights = _parent.Heights;
        try
        {
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
                            position = new[] { coordinates[i][0], heights[i], coordinates[i][1] },
                            scale = 1,
                            rotation = new[] { 0, 0, 0 }
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
        catch (Exception)
        {
            Console.WriteLine("Error while rendering trees");
        }
    }
}