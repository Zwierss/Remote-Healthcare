#nullable enable
using System.Security;
using DoctorApplication.commands;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using static DoctorLogic.State;
using ICommand = System.Windows.Input.ICommand;

namespace DoctorApplication.viewmodels;

public class AccountViewModel : ObservableObject, IWindow
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
        NavigationStore.Client.ViewModel = this;
        Create = new CreateCommand(this);
    }

    public void OnChangedValues(State state, string[]? args = null)
    {
        switch (state)
        {
            case Success:
                NavigationStore.CurrentViewModel = new BeginViewModel(NavigationStore);
                break;
            case Error:
                ErrorMessage = args![0];
                break;
        }
    }
}