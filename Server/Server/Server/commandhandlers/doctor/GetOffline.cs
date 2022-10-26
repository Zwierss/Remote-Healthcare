using Newtonsoft.Json.Linq;
using static Server.StorageManager;

namespace Server.commandhandlers.doctor;

public class GetOffline : ICommand
{
    /// <summary>
    /// It gets all the directories in the specified path, and sends them to the client
    /// </summary>
    /// <param name="JObject">The packet that was received.</param>
    /// <param name="Client">The client that sent the command</param>
    public void OnCommandReceived(JObject packet, Client parent)
    {
        string[] directoryNames = Directory.GetDirectories(PathDir, "", SearchOption.TopDirectoryOnly);
        List<string> directories = new List<string>();
        foreach (string s in directoryNames)
        {
            directories.Add(Path.GetFileName(s));
        }
        
        parent.SendMessage(PacketSender.SendReplacedObject("names", directories, 1, "doctor\\returnoffline.json")!);
    }
}