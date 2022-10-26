using System;
using System.Runtime.InteropServices;
using System.Security;
using ClientGUI.stores;
using ClientGUI.viewmodels;

namespace ClientGUI.commands;

public class LogInCommand : CommandBase
{
    
    private readonly BeginViewModel _view;

    public LogInCommand(BeginViewModel view)
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
        }

        if (string.IsNullOrEmpty(_view.Ip)) 
        {
            _view.ErrorMessage = "Voer een IP-adres in";
        }

        _view.NavigationStore.CurrentViewModel = new LoadingViewModel(_view.NavigationStore, _view.Username, password, _view.Ip, port, _view.FietsNr,_view.IsChecked);
    }
    
    
}