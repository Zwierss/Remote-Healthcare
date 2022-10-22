using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server;

public static class StorageManager
{
    private static readonly string PathDir =
        Environment.CurrentDirectory.Substring(0,
            Environment.CurrentDirectory.LastIndexOf("Server", StringComparison.Ordinal)) +
        "Server\\storage\\";

    private static JObject GetStorageFiles(string filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + filename)));
    }

    public static bool CheckIfDoctorExists(string username, string password)
    {
        string[][] accounts = GetStorageFiles("clients.json")["doctors"]!.ToObject<string[][]>()!;
        foreach (string[] account in accounts)
        {
            if(account[0] != username) continue;
            if (account[1] != password) return false;
            return true;
        }
        return false;
    }
}