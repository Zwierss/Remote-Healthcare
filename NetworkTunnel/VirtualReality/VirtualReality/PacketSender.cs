using System.Data.SqlTypes;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class PacketSender
{

    private static string _PATHDIR = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin", StringComparison.Ordinal)) + "JSON\\";
    
    public static JObject? SendReplacedObject<TR,TO>(string variable, TR replacement, int position, TO targetObject)
    {
        
        JObject? data = null;
        if(targetObject?.GetType() == typeof(string))
        {
            data = (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + targetObject)));
        }
        else if (targetObject?.GetType() == typeof(JObject))
        {
            data = targetObject as JObject;
        }

        JObject? currentObject = data;

        for (int i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        if (replacement is string)
        {
            currentObject![variable] = replacement as string;
        }
        else if (replacement is JObject)
        {
            currentObject![variable] = replacement as JObject;
        }
        else if (replacement is double)
        {
            string r = replacement.ToString()!;
            currentObject![variable] = (JToken)double.Parse(r);
        }

        return data;
    }

    public static JObject GetJson(string? filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + filename)));
    }

    public static JObject? GetJsonThroughTunnel<T>(T o, string id)
    {
        JObject? data = null;
        if (o! is string)
        {
            data = GetJson(o as string);
        }
        else if (o! is JObject)
        {
            data = o as JObject;
        }

        return SendReplacedObject<JObject, JObject>("data", data!, 1, SendReplacedObject<string, string>("dest", id, 1, "tunnelsend.json")!);
    }
}