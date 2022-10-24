using System.Windows.Input;
using ClientApplication;
using ClientGUI.commands;
using DoctorApplication.stores;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;
using static ClientApplication.State;

namespace ClientGUI.viewmodels;

public class SuccessViewModel : ObservableObject, IClientCallback
{

    public NavigationStore NavigationStore { get; set; }
    public ICommand Stop{ get; set; }

    private string _image = "resources/checkmark.png";
    public string Image
    {
        get => _image;
        set
        {
            _image = value;
            OnPropertyChanged();
        }
    }

    private string _message = "Gelukt! De fiets is klaar voor gebuik";
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    public SuccessViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        Stop = new StopCommand(this);
    }

    public void OnCallback(State state, string value = "")
    {
        switch (state)
        {
            case Error:
                Message = value;
                Image = "resources/warning.png";
                break;
        }
    }
}