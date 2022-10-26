using System;
using System.Security;
using ClientApplication;
using ClientGUI.commands;
using DoctorApplication.stores;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;
using static ClientApplication.State;

namespace ClientGUI.viewmodels;

public class BeginViewModel : ObservableObject, IClientCallback
{
    public NavigationStore NavigationStore { get; set; }

    public string Username { get; set; }

    private SecureString _secureString;
    public SecureString SecurePassword
    {
        get => _secureString;
        set => _secureString = value;
    }

    private string _ip = "localhost";
    public string Ip
    {
        get => _ip;
        set
        {
            _ip = value;
            OnPropertyChanged();
        }
    }

    private string _port = "6666";
    public string Port
    {
        get => _port;
        set
        {
            _port = value;
            OnPropertyChanged();
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    private string _fietsNr = "01140";
    public string FietsNr
    {
        get => _fietsNr;
        set
        {
            _fietsNr = value;
            OnPropertyChanged();
        }
    }

    private bool _isChecked;
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged();
        }
    }

    public ICommand LogIn { get; }
    public ICommand MakeNew { get; }

    public BeginViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        LogIn = new LogInCommand(this);
        MakeNew = new MakeAccountCommand(this);
    }

    public void OnCallback(State state, string value = "")
    {
        switch (state)
        {
            case Error:
                ErrorMessage = value;
                break;
            case Success:
                if (Username.ToLower() == "rick")
                {
                    NavigationStore.CurrentViewModel = new LoadingViewModel(NavigationStore, "rick");
                }
                else 
                {
                    NavigationStore.CurrentViewModel = new LoadingViewModel(NavigationStore, "load");
                }
                
                break;
        }
    }
}