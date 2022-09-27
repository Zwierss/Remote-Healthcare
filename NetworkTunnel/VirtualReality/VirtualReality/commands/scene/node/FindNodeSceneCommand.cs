using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.scene.node;

public class FindNodeSceneCommand : TunnelCallback
{
	public void OnCommandReceived(JObject o, Client parent)
	{
		if (o["data"]![0]!["name"]!.ToString() == "Camera")
		{
			parent.CameraId = o["data"]![0]!["uuid"]!.ToString();
			Console.WriteLine("setting CameraId to " + o["data"]![0]!["uuid"]!);
		}
		else if (o["data"]![0]!["name"]!.ToString() == "GroundPlane")
		{
			parent.TerrainId = o["data"]![0]!["uuid"]!.ToString();
		}
	}
}