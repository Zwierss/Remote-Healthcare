using Newtonsoft.Json.Linq;
using VirtualReality.commands.route;
using VirtualReality.commands.scene.node;
using VirtualReality.commands.scene.terrain;

namespace VirtualReality.commands.tunnel;

public class TunnelCommand : ICommand
{

    private readonly Dictionary<string, ITunnelCallback> _commands;

    /* Initializing the commands dictionary and calling the InitCommands function. */
    public TunnelCommand()
    {
        _commands = new Dictionary<string, ITunnelCallback>();
        InitCommands();
    }

    /// <summary>
    /// It loops through all the commands in the _commands dictionary and checks if the command id matches the id of the
    /// command that was received. If it does, it calls the OnCommandReceived function of the command
    /// </summary>
    /// <param name="JObject">The JSON object that was received from the server.</param>
    /// <param name="VRClient">The VRClient that sent the command</param>
    public void OnCommandReceived(JObject ob, VRClient vrClient)
    {
        foreach (var key in _commands.Keys.Where(key => key == ob["data"]!["data"]!["id"]!.ToObject<string>()))
        {
            _commands[key].OnCommandReceived(((JObject)ob["data"]!["data"]!)!, vrClient);
        }
    }

    /// <summary>
    /// It adds a bunch of commands to a dictionary
    /// </summary>
    private void InitCommands()
    {
    _commands.Add("scene/node/add", new AddNodeSceneCommand());
    _commands.Add("scene/node/find", new FindNodeSceneCommand());
    _commands.Add("scene/terrain/getheight", new GetheightTerrainSceneCommand());
    _commands.Add("route/add", new AddRouteCommand());
    }
}