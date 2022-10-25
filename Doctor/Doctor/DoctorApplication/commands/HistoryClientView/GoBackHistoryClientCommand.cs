using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.HistoryClientView;

public class GoBackHistoryClientCommand : CommandBase
{

    private HistoryClientViewModel _view;

    public GoBackHistoryClientCommand(HistoryClientViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.CurrentViewModel = new HistoryViewModel(_view.NavigationStore);
    }
}