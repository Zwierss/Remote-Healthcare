using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.ClientView;

public class SessionCommand : CommandBase
{

    private ClientViewModel _view;

    /* A constructor. */
    public SessionCommand(ClientViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// If the session is active, stop it, otherwise start it
    /// </summary>
    /// <param name="parameter">The parameter that was passed to the command.</param>
    /// <returns>
    /// The result of the method is being returned.
    /// </returns>
    public override void Execute(object parameter)
    {
        if (!_view.IsOnline)
        {
            _view.ErrorMessage = "Deze client is niet meer online";
            OverrideViews();
            return;
        }

        if (_view.SessionIsActive)
        {
            OverrideViews();
            _view.NavigationStore.Client.StopSession(_view.UserId);
        }
        else
        {
            _view.SessionBtn = "Stop sessie";
            _view.NavigationStore.Client.StartSession(_view.UserId);
        }

        _view.SessionIsActive = !_view.SessionIsActive;
    }

    /// <summary>
    /// It sets all the values of the view to "--"
    /// </summary>
    private void OverrideViews()
    {
        _view.Speed = "--";
        _view.Heartbeat = "--";
        _view.SpeedAvg = "--";
        _view.HeartbeatAvg = "--";
        _view.Distance = "--";
        _view.Time = "--";
        _view.SessionBtn = "Start sessie";
    }
}