using Newtonsoft.Json.Linq;
using static Server.StorageManager;

namespace Server.commandhandlers.doctor;

public class GetSessions: ICommand
{
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string client = packet["data"]!["client"]!.ToObject<string>()!;
        string[] directoryNames = Directory.GetDirectories(PathDir, "", SearchOption.TopDirectoryOnly);
        List<string> sessionNames = new List<string>();
        
        foreach (string s in directoryNames)
        {
            if (Path.GetFileName(s) != client) continue;
            string[] sessions = Directory.GetFiles(PathDir + client + "\\", "", SearchOption.TopDirectoryOnly);
            foreach (string session in sessions)
            {
                    sessionNames.Add(Path.GetFileName(session));
            }
            break;
        }
        parent.SendMessage(PacketSender.SendReplacedObject("files", sessionNames, 1, "doctor\\returnsessions.json")!);
    }
}