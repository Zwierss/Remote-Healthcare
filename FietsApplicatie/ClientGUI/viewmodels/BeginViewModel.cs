using System.Security;
using ClientGUI.commands;
using DoctorApplication.stores;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;

namespace ClientGUI.viewmodels;

public class BeginViewModel : ObservableObject
{
    private NavigationStore _navigationStore;

    public string Username { get; set; }

    private SecureString _secureString;
    public SecureString SecurePassword
    {
        get => _secureString;
        set => _secureString = value;
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

    public BeginViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        LogIn = new LogInCommand(this);
    }
}