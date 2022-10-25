using Newtonsoft.Json.Linq;
using static Server.StorageManager;

namespace Server.commandhandlers.doctor;

public class StartSession : ICommand
{
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