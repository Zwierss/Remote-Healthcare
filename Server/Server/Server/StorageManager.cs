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
        string json;
        using (StreamReader sr = new StreamReader(PathDir + filename))
        {
            json = sr.ReadToEnd();
             
        }
        return JObject.Parse(json);
    }

    public static void AddNewAccount(string username, string password, string type)
    {
        JObject accounts = GetStorageFiles("clients.json");
        List<string[]> accountTypes = accounts[type]!.ToObject<string[][]>()!.ToList();
        accountTypes.Add(new[]{username, password });
        JArray array = new JArray();
        foreach (string[] s in accountTypes)
        {
            array.Add(new JArray(s));
        }
        accounts[type] = array;

        File.WriteAllText(PathDir + "clients.json", accounts.ToString());
        using StreamWriter sw = File.CreateText(PathDir + "clients.json");
        using (JsonTextWriter writer = new JsonTextWriter(sw))
        {
            accounts.WriteTo(writer);
        }
    }

    public static bool CheckIfNewUsername(string username, string type)
    {
        string[][] accounts = GetStorageFiles("clients.json")[type]!.ToObject<string[][]>()!;
        foreach (string[] account in accounts)
        {
            if (account[0] == username) return false;
        }
        return true;
    }

    public static bool CheckIfAccountExists(string username, string password, string type)
    {
        string[][] accounts = GetStorageFiles("clients.json")[type]!.ToObject<string[][]>()!;
        foreach (string[] account in accounts)
        {
            if(account[0] != username) continue;
            if (account[1] != password) return false;
            return true;
        }
        return false;
    }

    public static bool CheckIfAlreadyOpen(string username, List<Client> clients, bool type)
    {
        if (type)
        {
            foreach (var c in clients)
            {
                if (!c.IsDoctor) continue;
                if (c.Uuid != username) continue;
                return false;
            }
        }
        else
        {
            foreach (var c in clients)
            {
                if (c.IsDoctor) continue;
                if (c.Uuid != username) continue;
                return false;
            }
        }

        return true;
    }
}