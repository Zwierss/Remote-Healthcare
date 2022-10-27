using System;
using DoctorApplication.stores;
using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.BeginView;

public class LogInCommand : CommandBase
{
    
    private readonly BeginViewModel _view;

    /* The constructor of the class. It is called when the class is instantiated. */
    public LogInCommand(BeginViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// It takes the input from the view, and passes it to the client
    /// </summary>
    /// <param name="parameter">The parameter passed to the command. In this case, it's the viewmodel.</param>
    /// <returns>
    /// The password is being returned.
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
            _view.ErrorMessage = "Voer alstublieft een (geldig) wachtwoord in";
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

        if (string.IsNullOrEmpty(_view.Username)) 
        {
            _view.ErrorMessage = "Vul een gebruikersnaam in";
            return;
        }

        if (string.IsNullOrEmpty(_view.Ip)) 
        {
            _view.ErrorMessage = "vul een IP adres in";
            return;
        }

        _view.NavigationStore.Client.SetupConnection(_view.Username, password, _view.Ip, port);
    }
}