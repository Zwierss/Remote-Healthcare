using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualReality;

public static class PacketSender
{

    private static readonly string PathDir = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("ClientGUI", StringComparison.Ordinal)) + "VirtualReality\\JSON\\";
    
    /// <summary>
    /// It takes a variable name, a replacement value, a position, and a target object, and returns the target object with
    /// the variable at the position replaced with the replacement value
    /// </summary>
    /// <param name="variable">The name of the variable you want to replace.</param>
    /// <param name="TR">The type of the replacement</param>
    /// <param name="position">The position of the object you want to replace.</param>
    /// <param name="TO">The object that you want to send to the function. This can be a string, a JObject, or a
    /// JArray.</param>
    /// <returns>
    /// A JObject
    /// </returns>
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
            case bool g:
                currentObject![variable] = g;
                break;
        }

        return data;
    }

    /// <summary>
    /// It takes a filename as a string, opens the file, reads the file, and returns the file as a JObject
    /// </summary>
    /// <param name="filename">The name of the file you want to read.</param>
    /// <returns>
    /// A JObject
    /// </returns>
    public static JObject GetJson(string? filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + filename)));
    }

    /// <summary>
    /// It takes an object, converts it to JSON, sends it through a tunnel, and returns the JSON object that was sent back
    /// </summary>
    /// <param name="T">The type of the object you're sending.</param>
    /// <param name="id">The id of the tunnel to send the data through.</param>
    /// <returns>
    /// A JObject
    /// </returns>
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