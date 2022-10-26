using Newtonsoft.Json.Linq;
using VirtualReality.commands.route;
using VirtualReality.commands.scene.node;
using VirtualReality.commands.scene.terrain;

namespace VirtualReality.commands.tunnel;

public class TunnelCommand : ICommand
{

    private readonly Dictionary<string, ITunnelCallback> _commands;

    public TunnelCommand()
    {
        _commands = new Dictionary<string, ITunnelCallback>();
        InitCommands();
    }

    public void OnCommandReceived(JObject ob, VRClient vrClient)
    {
        foreach (var key in _commands.Keys.Where(key => key == ob["data"]!["data"]!["id"]!.ToObject<string>()))
        {
            _commands[key].OnCommandReceived(((JObject)ob["data"]!["data"]!)!, vrClient);
        }
    }

    private void InitCommands()
    {
    _commands.Add("scene/node/add", new AddNodeSceneCommand());
    _commands.Add("scene/node/find", new FindNodeSceneCommand());
    _commands.Add("scene/terrain/getheight", new GetheightTerrainSceneCommand());
    _commands.Add("route/add", new AddRouteCommand());
    }
}