using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.BeginView;

public class MakeAccountCommand : CommandBase
{

    private BeginViewModel _view;

    /* A constructor. */
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