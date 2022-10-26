using ClientGUI.viewmodels;

namespace ClientGUI.commands;

public class MakeAccountCommand : CommandBase
{

    private BeginViewModel _view;

    /* A constructor that takes in a BeginViewModel and sets it to the private variable _view. */
    public MakeAccountCommand(BeginViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// It sets the current view model to a new instance of the AccountViewModel
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public override void Execute(object parameter)
    {
        _view.NavigationStore.CurrentViewModel = new AccountViewModel(_view.NavigationStore);
    }
}