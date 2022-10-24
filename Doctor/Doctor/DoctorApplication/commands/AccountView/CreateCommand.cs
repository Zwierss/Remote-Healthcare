using System;
using DoctorApplication.stores;
using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.AccountView;

public class CreateCommand : CommandBase
{

    private AccountViewModel _view;

    public CreateCommand(AccountViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        int port;
        string password;

        try
        {
            password = UtilStore.SecureStringToString(_view.SecurePassword);
        }
        catch (Exception)
        {
            _view.ErrorMessage = "Voer alstublieft een wachtwoord in";
            return;
        }

        try
        {
            port = int.Parse(_view.Port);
        }
        catch (Exception)
        {
            _view.ErrorMessage = "Port moet een getal zijn";
            return;
        }

        _view.NavigationStore.Client.CreateAccount(_view.Username, password, _view.Ip, port);
    }
}