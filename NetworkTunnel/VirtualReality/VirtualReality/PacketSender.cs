using System.Data.SqlTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class PacketSender
{

    private static string _PATHDIR = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin", StringComparison.Ordinal)) + "JSON\\";

    public static JObject SendReplacedObject(string variable, string replacement, int position, string filename)
    {
        JObject data = (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + filename)));
        JObject currentObject = data;
        for (int i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        currentObject![variable] = replacement;
        return data;
    }
    
    public static JObject SendReplacedObject(string variable, string replacement, int position, JObject targetObject)
    {
        JObject currentObject = targetObject;
        for (int i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        currentObject[variable] = replacement;
        return targetObject;
    }

    public static JObject SendReplacedObject(string variable, double replacement, int position, string filename)
    {
        JObject data = (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + filename)));
        JObject currentObject = data;
        for (int i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        currentObject![variable] = replacement;
        return data;
    }

    public static JObject SendReplacedObject(string variable, JObject replacement, int position, string filename)
    {
        JObject data = (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + filename)));
        JObject currentObject = data;
        for (int i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        currentObject[variable] = replacement;
        return data;
    }
    
    public static JObject SendReplacedObject(string variable, JObject replacement, int position, JObject targetObject)
    {
        JObject currentObject = targetObject;
        for (int i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        currentObject[variable] = replacement;
        return targetObject;
    }

    public static JObject GetJson(string filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(_PATHDIR + filename)));
    }

    public static JObject GetJsonThroughTunnel(string filename, string id)
    {
        return SendReplacedObject("data", GetJson(filename), 1, SendReplacedObject("dest", id, 1, "tunnelsend.json"));
    }
    public static JObject GetJsonThroughTunnel(JObject o, string id)
    {
        return SendReplacedObject("data", o, 1, SendReplacedObject("dest", id, 1, "tunnelsend.json"));
    }
}