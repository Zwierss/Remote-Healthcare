using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.ClientView;

public class EmergencyStopCommand : CommandBase
{
    private ClientViewModel _view;

    /* A constructor. */
    public EmergencyStopCommand(ClientViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// If the client is online, send an emergency stop command to the client
    /// </summary>
    /// <param name="parameter">The parameter passed to the command when it was executed.</param>
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