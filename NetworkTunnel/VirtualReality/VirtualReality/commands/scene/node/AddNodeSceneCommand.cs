using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.scene.node;

public class AddNodeSceneCommand : TunnelCallback
{
	public void OnCommandReceived(JObject o, Client parent)
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
		
	}
}