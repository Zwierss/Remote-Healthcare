using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.scene.node;

public class AddNodeSceneCommand : TunnelCallback
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
		// switch (o["data"]!["name"]!.ToString())
		// {
		// 	case "floor":
		// 		parent.TerrainId = o["data"]!["uuid"]!.ToString();
		// 		break;
		// 	case "bike":
		// 		parent.BikeId = o["data"]!["uuid"]!.ToString();
		// 		break;
		// 	case "panel":
		// 		parent.PanelId = o["data"]!["uuid"]!.ToString();
		// 		break;
		// }
	}
}