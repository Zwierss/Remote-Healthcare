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
    public NavigationStore NavigationStore { get; set; }

    public ICommand Create { get; }

    private string _username;
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
    public SecureString SecurePassword
    {
        get => _securePassword;
        set => _securePassword = value;
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

    public AccountViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        Create = new CreateCommand(this);
    }

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