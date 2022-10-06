using Newtonsoft.Json.Linq;

namespace VirtualReality.commands.scene.terrain;

public class GetheightTerrainSceneCommand : TunnelCallback
{
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