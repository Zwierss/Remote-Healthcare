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
    /* A property that is used to get and set the value of the SecureString. */
    public SecureString SecurePassword
    {
        get => _secureString;
        set => _secureString = value;
    }

    private string _ip = "localhost";
    /* A property that is used to get and set the value of the SecureString. */
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
    /* A property that is used to get and set the value of the SecureString. */
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
    /* This is a property that is used to get and set the value of the ErrorMessage. */
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
    /* This is a property that is used to get and set the value of the FietsNr. */
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
    /* This is a property that is used to get and set the value of the IsChecked. */
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged();
        }
    }

    private string _vr;
    /* This is a property that is used to get and set the value of the VR Client. */
    public string Vr 
    {
        get => _vr;
        set 
        {
            _vr = value;
            OnPropertyChanged();
        }
    }
        
    public ICommand LogIn { get; }
    public ICommand MakeNew { get; }

    /* This is the constructor of the BeginViewModel. It is used to initialize the NavigationStore, the Client and the
    Commands. */
    public BeginViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        LogIn = new LogInCommand(this);
        MakeNew = new MakeAccountCommand(this);
    }

    /// <summary>
    /// Called by active client
    /// `Callback` function
    /// </summary>
    /// <param name="State">This is the state of the callback.</param>
    /// <param name="value">The value of the callback.</param>
    public void OnCallback(State state, string value = "")
    {
        switch (state)
        {
            case Error:
                ErrorMessage = value;
                break;
        }
    }
}