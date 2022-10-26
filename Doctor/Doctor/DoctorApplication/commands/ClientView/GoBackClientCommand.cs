using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.ClientView;

public class GoBackClientCommand : CommandBase
{
    private ClientViewModel _view;

    /* A constructor. */
    public GoBackClientCommand(ClientViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// The function unsubscribes the user from the server and then navigates to the SelectionViewModel
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public override void Execute(object parameter)
    {
        _view.NavigationStore.Client.Unsubscribe(_view.UserId);
        _view.NavigationStore.CurrentViewModel = new SelectionViewModel(_view.NavigationStore);
    }
}