using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.HistoryView;

public class GoBackHistoryCommand : CommandBase
{

    private HistoryViewModel _view;

    /* A constructor. */
    public GoBackHistoryCommand(HistoryViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// It sets the current view model to a new instance of the SelectionViewModel
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public override void Execute(object parameter)
    {
        _view.NavigationStore.CurrentViewModel = new SelectionViewModel(_view.NavigationStore);
    }
}