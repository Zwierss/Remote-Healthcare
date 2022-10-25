using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Server.Cryptographer;

namespace Server;

public class StorageManager
{
    public static readonly string PathDir =
        Environment.CurrentDirectory.Substring(0,
            Environment.CurrentDirectory.LastIndexOf("Server", StringComparison.Ordinal)) +
        "Server\\storage\\";

    private JObject? _storeable;

    private JObject GetStorageFiles(string filename)
    {
        var stream = new FileStream(PathDir + filename, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[stream.Length];
        long bytesToRead = stream.Length;
        int bytesRead = 0;
        while (bytesToRead > 0)
        {
            int n = stream.Read(bytes, bytesRead, (int)bytesToRead);
            bytesRead += n;
            bytesToRead -= n;
        }
        stream.Flush();
        stream.Dispose();
        return GetDecryptedMessage(bytes);
    }

    private void WriteStorageFiles(JObject file, string filename)
    {
        byte[] a = GetEncryptedMessage(file);
        var stream = new FileStream(PathDir + filename,FileMode.Create, FileAccess.Write);
        stream.Write(a,0,a.Length);
        stream.Flush();
        stream.Dispose();
        _storeable = null;
    }

    public void AddNewAccount(string username, string password, string type)
    {
        JObject accounts = GetStorageFiles("accounts.acc");
        List<string[]> accountTypes = accounts[type]!.ToObject<string[][]>()!.ToList();
        accountTypes.Add(new[]{username, password });
        JArray array = new JArray();
        foreach (string[] s in accountTypes)
        {
            array.Add(new JArray(s));
        }
        accounts[type] = array;
        
        WriteStorageFiles(accounts, "accounts.acc");
        Console.WriteLine("test");

        if (type != "clients") return;
        if (!Directory.Exists(PathDir + "" + username))
        {
            Directory.CreateDirectory(PathDir + "" + username);
        }

    }

    public void StoreSession(double[] session, string path)
    {
        _storeable ??= GetJson("standard.json");

        double[][] data = _storeable["data"]!.ToObject<double[][]>()!;
        JArray array = new JArray();
        foreach (double[] gram in data)
        {
            JArray a = new JArray(gram);
            array.Add(a);
        }
        array.Add(new JArray(session));

        _storeable["data"] = array;
        Console.WriteLine(_storeable);
    }

    public void SaveSession(string path)
    {
        WriteStorageFiles(_storeable!, path);
    }

    public bool CheckIfNewUsername(string username)
    {
        string[][] clients = GetStorageFiles("accounts.acc")["clients"]!.ToObject<string[][]>()!;
        foreach (string[] account in clients)
        {
            if (account[0] == username) return false;
        }
        
        string[][] doctors = GetStorageFiles("accounts.acc")["doctors"]!.ToObject<string[][]>()!;
        foreach (string[] account in doctors)
        {
            if (account[0] == username) return false;
        }
        
        return true;
    }

    public bool CheckIfAccountExists(string username, string password, string type)
    {
        string[][] accounts = GetStorageFiles("accounts.acc")[type]!.ToObject<string[][]>()!;
        foreach (string[] account in accounts)
        {
            if(account[0] != username) continue;
            if (account[1] != password) return false;
            return true;
        }
        return false;
    }

    public bool CheckIfAlreadyOpen(string username, List<Client> clients)
    {
        foreach (Client c in clients)
        {
            if (c.Uuid == username) return false;
        }

        return true;
    }

    private JObject GetJson(string? filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + filename!)));
    }
}