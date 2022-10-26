using Newtonsoft.Json.Linq;
using VirtualReality.commands.tunnel;

namespace VirtualReality.commands.route;

public class AddRouteCommand : ITunnelCallback
{
	/// <summary>
	/// > This function is called when the client receives a command from the server
	/// </summary>
	/// <param name="JObject">The JSON object that was received from the server.</param>
	/// <param name="VRClient">The VRClient that received the command.</param>
	/// <summary>
	/// > This function is called when the client receives a command from the server
	/// </summary>
	/// <param name="JObject">The JSON object that was received from the server.</param>
	/// <param name="VRClient">The VRClient that received the command.</param>
	public void OnCommandReceived(JObject o, VRClient parent)
	{
		parent.RouteId = o["data"]!["uuid"]!.ToString();
	}
}