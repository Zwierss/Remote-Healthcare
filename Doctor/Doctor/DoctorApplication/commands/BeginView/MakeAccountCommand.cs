using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.BeginView;

public class MakeAccountCommand : CommandBase
{

    private BeginViewModel _view;

    public MakeAccountCommand(BeginViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        _view.NavigationStore.CurrentViewModel = new AccountViewModel(_view.NavigationStore);
    }
}