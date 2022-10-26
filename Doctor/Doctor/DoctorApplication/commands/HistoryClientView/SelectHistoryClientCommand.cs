using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.HistoryClientView;

public class SelectHistoryClientCommand : CommandBase
{

    private HistoryClientViewModel _view;

    /* A constructor. */
    public SelectHistoryClientCommand(HistoryClientViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// If the user has selected an item from the list, then navigate to the loading history view model
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    /// <returns>
    /// The current view model.
    /// </returns>
    public override void Execute(object parameter)
    {
        string item = _view.SelectedItem;
        if(string.IsNullOrEmpty(item)) return;
        _view.NavigationStore.CurrentViewModel = new LoadingHistoryViewModel(_view.NavigationStore, _view.Client, item);
    }
}