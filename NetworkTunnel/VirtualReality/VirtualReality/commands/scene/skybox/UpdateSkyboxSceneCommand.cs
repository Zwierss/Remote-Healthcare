using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class UpdateSkyboxSceneCommand : TunnelCallback
{

	public void OnCommandReceived(JObject ob, TunnelCommand parent)
	{
		Console.WriteLine("TimeJson: " + ob);
		Console.WriteLine("Done with command");
	}
}