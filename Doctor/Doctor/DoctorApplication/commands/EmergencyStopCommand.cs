using DoctorApplication.viewmodels;

namespace DoctorApplication.commands;

public class EmergencyStopCommand : CommandBase
{
    private ClientViewModel _view;

    public EmergencyStopCommand(ClientViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        if (_view.IsOnline)
        {
            _view.NavigationStore.Client.EmergencyStop(_view.UserId);
        }
        else
        {
            _view.ErrorMessage = "Deze client is niet meer online";
        }
    }
}