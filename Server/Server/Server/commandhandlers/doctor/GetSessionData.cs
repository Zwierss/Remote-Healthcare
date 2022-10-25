using Newtonsoft.Json.Linq;
using static Server.StorageManager;

namespace Server.commandhandlers.doctor;

public class GetSessionData : ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string client = packet["data"]!["client"]!.ToObject<string>()!;
        string file = packet["data"]!["file"]!.ToObject<string>()!;
        string[] directoryNames = Directory.GetDirectories(PathDir, "", SearchOption.TopDirectoryOnly);

        foreach (string s in directoryNames)
        {
            if (Path.GetFileName(s) != client) continue;
            string[] sessions = Directory.GetFiles(PathDir + client + "\\", "", SearchOption.TopDirectoryOnly);
            foreach (string session in sessions)
            {
                if(Path.GetFileName(session) != file) continue;
                JObject o = parent.StorageManager.GetStorageFiles( client+ "\\" + file);
                double[][] data = o["data"]!.ToObject<double[][]>()!;
                foreach (var d in data)
                {
                    Thread.Sleep(100);
                    parent.SendMessage(PacketSender.SendReplacedObject("values", d, 1, "doctor\\returnsessiondata.json")!);
                }

                break;
            }
            break;
        }
        Thread.Sleep(50);
        parent.SendMessage(PacketSender.GetJson("doctor\\loadingcomplete.json"));
    }
}