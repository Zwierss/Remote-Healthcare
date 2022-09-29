using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class AddRouteCommand : TunnelCallback
{
	public void OnCommandReceived(JObject o, VRClient parent)
	{
		parent.RouteId = o["data"]["uuid"].ToString();
	}
}