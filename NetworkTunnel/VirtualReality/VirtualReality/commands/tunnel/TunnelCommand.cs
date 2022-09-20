using Newtonsoft.Json.Linq;

namespace VirtualReality;

public class TunnelCommand : Command
{

    private  Dictionary<string, TunnelCallback> _commands;

    public TunnelCommand()
    {
        _commands = new();
        InitCommands();
    }

    public void OnCommandReceived(JObject ob, Client client)
    {
    }

    private void InitCommands()
    { _commands.Add("scene/get", new GetSceneCommand());
    _commands.Add("scene/reset", new ResetSceneCommand());
    _commands.Add("scene/save", new SaveSceneCommand());
    _commands.Add("scene/load", new LoadSceneCommand());
    _commands.Add("scene/raycast", new RaycastSceneCommand());
    _commands.Add("scene/node/add", new AddNodeSceneCommand());
    _commands.Add("scene/node/moveto", new MovetoNodeSceneCommand());
    _commands.Add("scene/node/update", new UpdateNodeSceneCommand());
    _commands.Add("scene/node/delete", new DeleteNodeSceneCommand());
    _commands.Add("scene/node/addlayer", new AddlayerNodeSceneCommand());
    _commands.Add("scene/node/dellayer", new DellayerNodeSceneCommand());
    _commands.Add("scene/node/find", new FindNodeSceneCommand());
    _commands.Add("scene/terrain/add", new AddTerrainSceneCommand());
    _commands.Add("scene/terrain/update", new UpdateTerrainSceneCommand());
    _commands.Add("scene/terrain/delete", new DeleteTerrainSceneCommand());
    _commands.Add("scene/terrain/getheight", new GetheightTerrainSceneCommand());
    _commands.Add("scene/panel/clear", new ClearPanelSceneCommand());
    _commands.Add("scene/panel/drawlines", new DrawlinesPanelSceneCommand());
    _commands.Add("scene/panel/drawtext", new DrawtextPanelSceneCommand());
    _commands.Add("scene/panel/image", new ImagePanelSceneCommand());
    _commands.Add("scene/panel/setclearcolor", new SetclearcolorPanelSceneCommand());
    _commands.Add("scene/panel/sawp", new SawpPanelSceneCommand());
    _commands.Add("scene/skybox/settime", new SettimeSkyboxSceneCommand());
    _commands.Add("scene/skybox/update", new UpdateSkyboxSceneCommand());
    _commands.Add("scene/road/add", new AddRoadSceneCommand());
    _commands.Add("scene/road/update", new UpdateRoadSceneCommand());
    _commands.Add("route/add", new AddRouteCommand());
    _commands.Add("route/update", new UpdateRouteCommand());
    _commands.Add("route/delete", new DeleteRouteCommand());
    _commands.Add("route/follow", new FollowRouteCommand());
    _commands.Add("route/follow/speed", new SpeedFollowRouteCommand());
    _commands.Add("route/show", new ShowRouteCommand());
    }
}