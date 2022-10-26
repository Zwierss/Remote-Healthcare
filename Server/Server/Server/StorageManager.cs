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

    /// <summary>
    /// It reads the file, decrypts it, and returns the decrypted message
    /// </summary>
    /// <param name="filename">The name of the file to be read.</param>
    /// <returns>
    /// The file is being read and decrypted.
    /// </returns>
    public JObject GetStorageFiles(string filename)
    {
        Console.WriteLine(filename);
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

    /// <summary>
    /// It takes a JObject and a filename, encrypts the JObject, and writes it to a file
    /// </summary>
    /// <param name="JObject">The JObject to be encrypted</param>
    /// <param name="filename">The name of the file to be written.</param>
    private void WriteStorageFiles(JObject file, string filename)
    {
        byte[] a = GetEncryptedMessage(file);
        var stream = new FileStream(PathDir + filename,FileMode.Create, FileAccess.Write);
        stream.Write(a,0,a.Length);
        stream.Flush();
        stream.Dispose();
        _storeable = null;
    }

    /// <summary>
    /// It adds a new account to the accounts.acc file
    /// </summary>
    /// <param name="username">The username of the account</param>
    /// <param name="password">The password of the account</param>
    /// <param name="type">The type of account you want to add.</param>
    /// <returns>
    /// A JObject
    /// </returns>
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

    /// <summary>
    /// It takes a session and adds it to the data array in the JSON file
    /// </summary>
    /// <param name="session">The session to store</param>
    /// <param name="path">The path to the json file.</param>
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

    /// <summary>
    /// > Save the current session to the specified path
    /// </summary>
    /// <param name="path">The path to the folder where the files will be saved.</param>
    public void SaveSession(string path)
    {
        WriteStorageFiles(_storeable!, path);
    }

    /// <summary>
    /// It checks if the username is already taken
    /// </summary>
    /// <param name="username">The username to check if it's new</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
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

    /// <summary>
    /// It checks if the account exists
    /// </summary>
    /// <param name="username">The username of the account</param>
    /// <param name="password">The password of the account</param>
    /// <param name="type">The type of account you want to check.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
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

    /// <summary>
    /// It checks if the username is already in use by another client
    /// </summary>
    /// <param name="username">The username of the client</param>
    /// <param name="clients">The list of clients that are currently connected to the server.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    public bool CheckIfAlreadyOpen(string username, List<Client> clients)
    {
        foreach (Client c in clients)
        {
            if (c.Uuid == username) return false;
        }

        return true;
    }

    /// <summary>
    /// It takes a filename as a string, opens the file, reads the file, and returns the file as a JObject
    /// </summary>
    /// <param name="filename">The name of the file to be read.</param>
    /// <returns>
    /// A JObject
    /// </returns>
    private JObject GetJson(string? filename)
    {
        return (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + filename!)));
    }
}