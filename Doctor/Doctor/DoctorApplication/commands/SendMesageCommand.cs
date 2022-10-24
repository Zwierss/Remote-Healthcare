using DoctorApplication.viewmodels;

namespace DoctorApplication.commands;

public class SendMesageCommand : CommandBase
{

    private ClientViewModel _view;

    public SendMesageCommand(ClientViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        string message = _view.DoctorMsg;
        _view.NavigationStore.Client.SendDoctorMessage(_view.UserId, message);
    }
}