using Newtonsoft.Json.Linq;
using VirtualReality.commands.tunnel;

namespace VirtualReality.commands.scene.node;

public class AddNodeSceneCommand : ITunnelCallback
{
	/// <summary>
	/// If the name of the object is "floor", then the terrainId is set to the uuid of the object. If the name of the object is
	/// "bike", then the bikeId is set to the uuid of the object. If the name of the object is "Panel", then the panelId is set
	/// to the uuid of the object
	/// </summary>
	/// <param name="JObject">The JSON object that was received from the server.</param>
	/// <param name="VRClient">The VRClient object that is running the server.</param>
	public void OnCommandReceived(JObject o, VRClient parent)
	{
		if (o["data"]!["name"]!.ToString() == "floor")
		{
			Console.WriteLine("Changed floor");
			parent.TerrainId = o["data"]!["uuid"]!.ToString();
		}
		else if (o["data"]?["name"]!.ToString() == "bike")
		{
			Console.WriteLine("Changed bike");
			parent.BikeId = o["data"]!["uuid"]!.ToString();
		}
		else if (o["data"]!["name"]!.ToString() == "Panel")
		{
			Console.WriteLine("add panel uuid");
			parent.PanelId = o["data"]!["uuid"]!.ToString();
		}
	}
}