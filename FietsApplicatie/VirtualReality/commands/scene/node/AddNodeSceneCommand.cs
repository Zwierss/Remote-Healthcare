using Newtonsoft.Json.Linq;
using VirtualReality.commands.tunnel;

namespace VirtualReality.commands.scene.node;

public class AddNodeSceneCommand : ITunnelCallback
{
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