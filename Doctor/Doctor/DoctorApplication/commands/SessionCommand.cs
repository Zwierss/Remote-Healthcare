using DoctorApplication.viewmodels;

namespace DoctorApplication.commands;

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
            return;
        }

        if (_view.SessionIsActive)
        {
            _view.Speed = "--";
            _view.Heartbeat = "--";
            _view.SpeedAvg = "--";
            _view.HeartbeatAvg = "--";
            _view.Distance = "--";
            _view.Time = "--";
            _view.SessionBtn = "Start sessie";
            _view.NavigationStore.Client.StopSession(_view.UserId);
        }
        else
        {
            _view.SessionBtn = "Stop sessie";
            _view.NavigationStore.Client.StartSession(_view.UserId);
        }

        _view.SessionIsActive = !_view.SessionIsActive;
    }
}