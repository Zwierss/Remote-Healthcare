#nullable enable
using System;
using System.Runtime.InteropServices;
using System.Security;
using DoctorApplication.commands;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ICommand = System.Windows.Input.ICommand;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels;

public class BeginViewModel : ObservableObject, IWindow
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

    public ICommand LogIn { get; }
    public ICommand MakeNew { get; }

    public BeginViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
        LogIn = new LogInCommand(this);
        MakeNew = new MakeAccountCommand(this);
    }

    public void OnChangedValues(State state, string[]? args = null)
    {
        switch (state)
        {
            case Success:
                NavigationStore.CurrentViewModel = new SelectionViewModel(NavigationStore);
                ErrorMessage = "";
                break;
            case Error:
                ErrorMessage = args![0];
                break;
        }
    }
}