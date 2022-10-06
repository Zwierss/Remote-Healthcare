using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.scene.node;

public class FindNodeSceneCommand : TunnelCallback
{
	public void OnCommandReceived(JObject o, VRClient parent)
	{
		Console.WriteLine(o.ToString());
		switch (o["data"]![0]!["name"]!.ToString())
		{
			case "Camera":
				parent.CameraId = o["data"]![0]!["uuid"]!.ToString();
				Console.WriteLine("setting CameraId to " + o["data"]![0]!["uuid"]!);
				break;
			case "GroundPlane":
				parent.TerrainId = o["data"]![0]!["uuid"]!.ToString();
				break;
			case "Head":
				parent.HeadId = o["data"]![0]!["uuid"]!.ToString();
				break;
		}
	}
}