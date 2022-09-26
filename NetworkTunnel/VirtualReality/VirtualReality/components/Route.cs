namespace VirtualReality.components;

public class Route
{

    private Client _parent;

    public Route(Client parent)
    {
        _parent = parent;
    }

    public void CreateRoute()
    {
        // _parent.SendData(PacketSender.GetJsonThroughTunnel("route\\addroute.json", _parent._tunnelID));
        // Thread.Sleep(1000);
        // _parent.SendData(PacketSender.GetJsonThroughTunnel(PacketSender.SendReplacedObject(
        //     "route", _parent._routeID, 1, "scene\\road\\addroadscene.json"
        // ), _parent._tunnelID));
        double x = 0.0;
        double y = 0.0;

        List<double[]> coordinates = new();
        
        for (int i = 0; i < 200; i++)
        {

            coordinates.Add(new[]{x,y});
            
            if (i < 50)
            {
                x += 1.0;
            }
            else if (i < 100)
            {
                y += 1.0;
            }
            else if (i < 150)
            {
                x -= 1.0;
            }
            else
            {
                y -= 1.0;
            }
        }

        _parent.sendTunnel("scene/terrain/getheight", new {
            positions = coordinates.Select(a => new[]{(float)a[0], (float)a[1]}.ToArray())
        });
        
        
    }
}