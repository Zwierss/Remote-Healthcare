using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class GetheightTerrainSceneCommand : TunnelCallback
{
	public void OnCommandReceived(JObject o, Client parent)
	{
		parent._heights = o["data"]["height"].Cast<float>().ToArray();
	}
}