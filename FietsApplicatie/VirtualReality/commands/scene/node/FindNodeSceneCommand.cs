using Newtonsoft.Json.Linq;
using VirtualReality.commands.tunnel;

namespace VirtualReality.commands.scene.node;

public class FindNodeSceneCommand : ITunnelCallback
{
	/// <summary>
	/// If the name of the object is "Camera", set the CameraId to the UUID of the object
	/// </summary>
	/// <param name="JObject">The JSON object that was received from the server.</param>
	/// <param name="VRClient">The parent class that contains the VRClient object.</param>
	public void OnCommandReceived(JObject o, VRClient parent)
	{
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