namespace VirtualReality.components;

public class Skybox
{
    public bool _set { get; set; }
    private Client _parent;
    private double _count;
    
    public Skybox(Client parent)
    {
        _set = false;
        _parent = parent;
        _count = 0.0;
    }

    public void update()
    {
        while (true)
        {
            if (!_set)
            {
                _set = true;
                //_parent.SendData(PacketSender.GetJsonThroughTunnel("scene\\skybox\\updateskyboxscene.json", _parent._tunnelID));
                
            }
            _parent.SendData(PacketSender.GetJsonThroughTunnel(PacketSender.SendReplacedObject("time", _count, 1, "scene\\skybox\\settimeskyboxscene.json"), _parent._tunnelID));
            _count += 0.001;
            Thread.Sleep(1);
            
        }
    }
}