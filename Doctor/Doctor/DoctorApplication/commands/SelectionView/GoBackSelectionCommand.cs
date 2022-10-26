using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.SelectionView;

public class GoBackSelectionCommand : CommandBase
{
    private SelectionViewModel _view;

    /* A constructor. */
    public GoBackSelectionCommand(SelectionViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// > The function stops the current navigation and sets the current view model to the BeginViewModel
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public override void Execute(object parameter)
    {
        _view.NavigationStore.Client.Stop(true);
        _view.NavigationStore.CurrentViewModel = new BeginViewModel(_view.NavigationStore);
    }
}