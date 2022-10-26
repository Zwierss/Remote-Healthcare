using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.HistoryView;

public class SelectHistoryCommand : CommandBase
{

    private HistoryViewModel _view;

    /* A constructor. */
    public SelectHistoryCommand(HistoryViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// If the user has selected an item in the list, then navigate to the HistoryClientViewModel
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    /// <returns>
    /// The selected item from the listbox.
    /// </returns>
    public override void Execute(object parameter)
    {
        string item = _view.SelectedItem;
        if(string.IsNullOrEmpty(item)) return;
        _view.NavigationStore.CurrentViewModel = new HistoryClientViewModel(_view.NavigationStore, item);
    }
}