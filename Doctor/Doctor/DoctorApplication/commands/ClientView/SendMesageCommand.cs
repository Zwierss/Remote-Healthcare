using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.ClientView;

public class SendMesageCommand : CommandBase
{

    private ClientViewModel _view;

    /* A constructor. */
    public SendMesageCommand(ClientViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// The function checks if the client is online, if so, it sends the message to the client
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
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