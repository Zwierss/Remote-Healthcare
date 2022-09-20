namespace VirtualReality.components;

public class Skybox : Updatable
{
    private Client _parent;
    private bool _hasSetTime;
    
    public Skybox(Client parent)
    {
        _parent = parent;
        _hasSetTime = false;
    }


    public void update()
    {
        if (!_hasSetTime)
        {
            _hasSetTime = true;
            _parent.SendData(PacketSender.GetJsonThroughTunnel("scene\\skybox\\settimeskyboxscene.json", _parent._tunnelID));
        }

        //_parent.SendData(PacketSender.GetJsonThroughTunnel("scene\\skybox\\settimeskyboxscene.json", _parent._tunnelID));
    }
}