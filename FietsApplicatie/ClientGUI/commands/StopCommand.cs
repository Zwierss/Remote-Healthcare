using ClientGUI.viewmodels;

namespace ClientGUI.commands;

public class StopCommand : CommandBase
{

    private readonly SuccessViewModel _view;

    public StopCommand(SuccessViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.Client.Stop();
        _view.NavigationStore.CurrentViewModel = new BeginViewModel(_view.NavigationStore);
    }
}