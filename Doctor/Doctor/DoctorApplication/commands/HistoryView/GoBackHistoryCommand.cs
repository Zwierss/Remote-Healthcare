using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.HistoryView;

public class GoBackHistoryCommand : CommandBase
{

    private HistoryViewModel _view;

    public GoBackHistoryCommand(HistoryViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.CurrentViewModel = new SelectionViewModel(_view.NavigationStore);
    }
}