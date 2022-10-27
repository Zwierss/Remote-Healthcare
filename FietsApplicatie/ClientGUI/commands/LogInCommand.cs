using System;
using System.Runtime.InteropServices;
using System.Security;
using ClientGUI.stores;
using ClientGUI.viewmodels;

namespace ClientGUI.commands;

public class LogInCommand : CommandBase
{
    
    private readonly BeginViewModel _view;

    /* A constructor. */
    public LogInCommand(BeginViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// It checks if the user has entered a valid username, password, ip address, port and bike number. If so, it will
    /// navigate to the loading screen.
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    /// <returns>
    /// The current viewmodel is being returned.
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

        if (string.IsNullOrEmpty(_view.FietsNr)) 
        {
            _view.ErrorMessage = "Voer een geldig fietsnummer in";
            return;
        }

        if (string.IsNullOrEmpty(_view.Username)) 
        {
            _view.ErrorMessage = "Voer een gebruikersnaam in";
            return;
        }

        if (string.IsNullOrEmpty(_view.Ip)) 
        {
            _view.ErrorMessage = "Voer een IP-adres in";
            return;
        }

        if (string.IsNullOrEmpty(_view.Vr)) 
        {
            _view.Vr = Environment.UserName;
            
        }

        _view.NavigationStore.CurrentViewModel = new LoadingViewModel(_view.NavigationStore, _view.Username, password, _view.Ip, port, _view.FietsNr,_view.IsChecked, _view.Vr);
    }
    
    
}