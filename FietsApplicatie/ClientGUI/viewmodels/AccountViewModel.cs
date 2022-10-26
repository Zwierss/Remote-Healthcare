using System.Security;
using ClientApplication;
using ClientGUI.commands;
using DoctorApplication.stores;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;
using static ClientApplication.State;

namespace ClientGUI.viewmodels;

public class AccountViewModel : ObservableObject, IClientCallback
{
    public NavigationStore NavigationStore { get; }

    public ICommand Create { get; }

    private string _username;
    /* A property that is used to bind the username to the textbox in the view. */
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }
    
    private SecureString _securePassword;
    /* A property that is used to bind the password to the textbox in the view. */
    public SecureString SecurePassword
    {
        get => _securePassword;
        set => _securePassword = value;
    }
    
    private string _ip = "localhost";
    /* This is a property that is used to bind the ip to the textbox in the view. */
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
    /* This is a property that is used to bind the port to the textbox in the view. */
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
    /* This is a property that is used to bind the error message to the textbox in the view. */
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    /* This is the constructor of the AccountViewModel. It is used to create a new instance of the AccountViewModel. */
    public AccountViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        Create = new CreateCommand(this);
    }

    /// <summary>
    /// > The function is called when the user clicks the "Login" button
    /// </summary>
    /// <param name="State">This is an enum that you can use to determine what state the callback is in.</param>
    /// <param name="value">The value of the callback.</param>
    public void OnCallback(State state, string value = "")
    {
        switch (state)
        {
            case Success:
                NavigationStore.CurrentViewModel = new BeginViewModel(NavigationStore);
                break;
            case Error:
                ErrorMessage = value;
                break;
        }
    }
}