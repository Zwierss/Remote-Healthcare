using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.SelectionView;

public class GoBackSelectionCommand : CommandBase
{
    private SelectionViewModel _view;

    public GoBackSelectionCommand(SelectionViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.Client.Stop();
        _view.NavigationStore.CurrentViewModel = new BeginViewModel(_view.NavigationStore);
    }
}