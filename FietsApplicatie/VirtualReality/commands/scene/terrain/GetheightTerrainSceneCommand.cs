using Newtonsoft.Json.Linq;
using VirtualReality.commands.tunnel;

namespace VirtualReality.commands.scene.terrain;

public class GetheightTerrainSceneCommand : ITunnelCallback
{
	/// <summary>
	/// It takes the heights array from the JSON object and assigns it to the VRClient's Heights property
	/// </summary>
	/// <param name="JObject">The JSON object that was received from the server.</param>
	/// <param name="VRClient">The VRClient that sent the command.</param>
	public void OnCommandReceived(JObject o, VRClient parent)
	{
		try
		{
			int amount = o["data"]!["heights"]!.Count();
			float[] heights = new float[amount];
			for (int i = 0; i < amount; i++)
			{
				heights[i] = o["data"]!["heights"]![i]!.ToObject<float>();
			}

			parent.Heights = heights;
		}
		catch
		{
			Console.WriteLine("error");
		}
	}
}