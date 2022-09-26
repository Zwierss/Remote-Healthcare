namespace VirtualReality.components;

public class Node
{
    private Client _parent;

    public Node(Client parent)
    {
        _parent = parent;
    }

    public void CreateNode()
    {
        _parent.sendTunnel("scene/node/add", new {
            name = "floor",
            components = new
            {
                transform = new
                {
                    position = new[] { -128, -10, -128 },
                    scale = 1
                },
                terrain = new
                {

                }
            }
        });    
    }
}