using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.ClientView;

public class SendMesageCommand : CommandBase
{

    private ClientViewModel _view;

    public SendMesageCommand(ClientViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        if (_view.IsOnline)
        {
            string message = _view.DoctorMsg;
            _view.NavigationStore.Client.SendDoctorMessage(_view.UserId, message);
        }
        else
        {
            _view.ErrorMessage = "Deze client is niet meer online";
        }
    }
}