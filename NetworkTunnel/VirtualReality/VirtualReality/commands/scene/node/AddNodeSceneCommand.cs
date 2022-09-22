using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class AddNodeSceneCommand : TunnelCallback
{
	public void OnCommandReceived(JObject o, Client parent)
	{
		parent._nodeID = o["data"]["uuid"].ToString();
	}
}