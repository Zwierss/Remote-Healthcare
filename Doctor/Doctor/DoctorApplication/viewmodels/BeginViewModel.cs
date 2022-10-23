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
        NavigationStore = navigationStore;
        LogIn = new LogInCommand(this);
    }

    public void OnChangedValues(State state, string value = "")
    {
        switch (state)
        {
            case Success:
                NavigationStore.CurrentViewModel = new SelectionViewModel(NavigationStore);
                break;
        }

        
        ErrorMessage = value;
    }
}