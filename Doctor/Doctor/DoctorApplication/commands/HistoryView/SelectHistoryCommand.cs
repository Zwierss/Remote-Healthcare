using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.HistoryView;

public class SelectHistoryCommand : CommandBase
{

    private HistoryViewModel _view;

    public SelectHistoryCommand(HistoryViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        string item = _view.SelectedItem;
        if(string.IsNullOrEmpty(item)) return;
        _view.NavigationStore.CurrentViewModel = new HistoryClientViewModel(_view.NavigationStore, item);
    }
}