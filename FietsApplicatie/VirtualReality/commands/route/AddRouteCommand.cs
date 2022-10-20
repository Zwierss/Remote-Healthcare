using Newtonsoft.Json.Linq;
using VirtualReality.commands.tunnel;

namespace VirtualReality.commands.route;

public class AddRouteCommand : ITunnelCallback
{
	public void OnCommandReceived(JObject o, VRClient parent)
	{
		parent.RouteId = o["data"]!["uuid"]!.ToString();
	}
}