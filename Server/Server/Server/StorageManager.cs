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

    public static void AddNewAccount(string username, string password, string type)
    {
        JObject accounts = GetStorageFiles("clients.json");
        List<string[]> accountTypes = accounts[type]!.ToObject<string[][]>()!.ToList();
        string[] newAccount = { username, password };
        accountTypes.Add(newAccount);
        JArray array = new JArray(accountTypes);
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

    public static bool CheckIfAlreadyOpen(string username, List<Client> clients)
    {
        foreach (Client c in clients)
        {
            if (c.Uuid == username)
            {
                return false;
            }
        }

        return true;
    }
}