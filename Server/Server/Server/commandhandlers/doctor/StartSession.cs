using Newtonsoft.Json.Linq;
using static Server.StorageManager;

namespace Server.commandhandlers.doctor;

public class StartSession : ICommand
{
    /// <summary>
    /// It checks if the client is already in a session, and if not, it creates a new session for them
    /// </summary>
    /// <param name="JObject">The packet that was received from the client.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string username = packet["data"]!["client"]!.ToObject<string>()!;
        string count = Directory.GetFiles(PathDir + "" + username).Length.ToString();
        string zeros = "";
        int c = 4 - count.Length;
        for (int i = 0; i < c; i++)
        {
            zeros += "0";
        }

        foreach (Client client in parent.Parent.Clients)
        {
            Console.WriteLine(username + " " + client.Uuid);
            if(client.Uuid != username)continue;
            client.SessionActive = true;
            client.CurrentSessionTitle = username + "\\" + username + "-" + zeros + count + ".datastore";
        }
        new Switch().OnCommandReceived(packet, parent);
    }
}