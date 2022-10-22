using System;
using System.Runtime.InteropServices;
using System.Security;
using DoctorApplication.commands;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ICommand = System.Windows.Input.ICommand;

namespace DoctorApplication.viewmodels;

public class BeginViewModel : ObservableObject, IWindow
{
    private DoctorClient _client;
    private NavigationStore _navigationStore;
    public DoctorClient Client { get; set; }

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

    public void OnChangedValues(string value = "")
    {
        if (value == "")
        {
            _navigationStore.CurrentViewModel = new SelectionViewModel();
        }

        ErrorMessage = value;
    }
}