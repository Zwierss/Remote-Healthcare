using System.Data.SqlTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class PacketHandler
{

    private static string _PATHDIR = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin", StringComparison.Ordinal)) + "JSON\\";

    public static JObject ReplaceObject(string variable, string replacement, int position, string filename)
    {
        JObject data = (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + filename)));
        JObject currentObject = data;
        for (int i = 0; i < position; i++)
        {
            currentObject = (JObject?)currentObject["data"];
        }

        currentObject[variable] = replacement;
        return data;
    }

    public static JObject GetJson(string filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + filename)));
    }
}