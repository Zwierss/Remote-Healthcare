using System;
using ClientGUI.stores;
using ClientGUI.viewmodels;

namespace ClientGUI.commands;

public class CreateCommand : CommandBase
{

    private AccountViewModel _view;

    /* A constructor. */
    public CreateCommand(AccountViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// It takes the data from the view, and sends it to the server
    /// </summary>
    /// <param name="parameter">The parameter passed to the command. In this case, it's the view.</param>
    /// <returns>
    /// The password is being returned as a string.
    /// </returns>
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