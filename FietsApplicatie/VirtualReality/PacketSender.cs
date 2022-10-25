using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public static class PacketSender
{

    private static readonly string PathDir = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("ClientGUI", StringComparison.Ordinal)) + "VirtualReality\\JSON\\";
    
    public static JObject? SendReplacedObject<TR,TO>(string variable, TR replacement, int position, TO targetObject)
    {
        JObject? data = targetObject switch
        {
            string => (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + targetObject))),
            JObject jObject => jObject,
            _ => null
        };

        JObject? currentObject = data;

        for (int i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        switch (replacement)
        {
            case string s:
                currentObject![variable] = s;
                break;
            case JObject jObject:
                currentObject![variable] = jObject;
                break;
            case double d:
                currentObject![variable] = d;
                break;
            case float[][] f:
                JArray a = new JArray();
                foreach (var t in f)
                {
                    JArray j = new JArray(t);
                    a.Add(j);
                }

                currentObject![variable] = a;
                break;
            case int j:
                currentObject![variable] = j;
                break;
            case int[] i:
                JArray b = new JArray(i);
                currentObject![variable] = b;
                break;
            case double[][] m:
                JArray l = new JArray();
                foreach (var t in m)
                {
                    JArray j = new JArray(t);
                    l.Add(j);
                }
                currentObject![variable] = l;
                break;
        }

        return data;
    }

    public static JObject GetJson(string? filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + filename)));
    }

    public static JObject? GetJsonThroughTunnel<T>(T o, string id)
    {
        JObject? data = o! switch
        {
            string => GetJson(o as string),
            JObject => o as JObject,
            _ => null
        };

        return SendReplacedObject<JObject, JObject>("data", data!, 1, SendReplacedObject<string, string>("dest", id, 1, "tunnelsend.json")!);
    }
}