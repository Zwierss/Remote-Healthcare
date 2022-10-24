using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.ClientView;

public class SessionCommand : CommandBase
{

    private ClientViewModel _view;

    public SessionCommand(ClientViewModel view)
    {
        _view = view;
    }

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

    public void OverrideViews()
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