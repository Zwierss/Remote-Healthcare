using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.HistoryClientView;

public class SelectHistoryClientCommand : CommandBase
{

    private HistoryClientViewModel _view;

    public SelectHistoryClientCommand(HistoryClientViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        string item = _view.SelectedItem;
        if(string.IsNullOrEmpty(item)) return;
        _view.NavigationStore.CurrentViewModel = new LoadingHistoryViewModel(_view.NavigationStore, _view.Client, item);
    }
}