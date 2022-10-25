using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.SelectionView;

public class ViewHistoricCommand : CommandBase
{

    private SelectionViewModel _view;

    public ViewHistoricCommand(SelectionViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.CurrentViewModel = new HistoryViewModel(_view.NavigationStore);
    }
}