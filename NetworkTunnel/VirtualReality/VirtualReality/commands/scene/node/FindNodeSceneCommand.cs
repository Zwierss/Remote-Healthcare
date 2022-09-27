using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class FindNodeSceneCommand : TunnelCallback
{
	public void OnCommandReceived(JObject o, Client parent)
	{
		// if (o["data"]!["name"]!.ToString() == "Camera")
		// {
		// 	parent.CameraId = o["data"]!["uuid"]!.ToString();
		// }
	}
}