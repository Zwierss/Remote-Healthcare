using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.SelectionView;

public class ViewHistoricCommand : CommandBase
{

    private SelectionViewModel _view;

    /* A constructor. */
    public ViewHistoricCommand(SelectionViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// It sets the current view model to a new instance of the HistoryViewModel
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public override void Execute(object parameter)
    {
        _view.NavigationStore.CurrentViewModel = new HistoryViewModel(_view.NavigationStore);
    }
}