using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.ClientView;

public class GoBackClientCommand : CommandBase
{
    private ClientViewModel _view;

    public GoBackClientCommand(ClientViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.Client.Unsubscribe(_view.UserId);
        _view.NavigationStore.CurrentViewModel = new SelectionViewModel(_view.NavigationStore);
    }
}