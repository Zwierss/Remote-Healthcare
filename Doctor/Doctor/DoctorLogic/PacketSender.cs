using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DoctorLogic;

public static class PacketSender
{
    private static readonly string PathDir = Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf("Doctor", StringComparison.Ordinal)) + "DoctorLogic\\commands\\";
    
    /// <summary>
    /// It takes a variable name, a replacement value, a position, and a target object, and returns the target object with
    /// the variable at the position replaced with the replacement value
    /// </summary>
    /// <param name="variable">The name of the variable you want to replace.</param>
    /// <param name="TR">The type of the replacement</param>
    /// <param name="position">The position of the object you want to replace.</param>
    /// <param name="TO">The object that you want to modify. This can be a string (the name of the file), a JObject, or a
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
    /// <summary>
    /// It takes a filename as a string, opens the file, reads the file, and returns the file as a JObject
    /// </summary>
    /// <param name="filename">The name of the file you want to read.</param>
    /// <returns>
    /// A JObject
    /// </returns>
                break;
            case int j:
                currentObject![variable] = j;
                break;
            case bool g:
                currentObject![variable] = g;
                break;
        }

        return data;
    }

    public static JObject GetJson(string? filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + filename)));
    }
}