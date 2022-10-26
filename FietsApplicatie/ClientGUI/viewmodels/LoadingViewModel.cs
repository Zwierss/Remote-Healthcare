using System;
using ClientApplication;
using DoctorApplication.stores;
using MvvmHelpers;
using static ClientApplication.State;

namespace ClientGUI.viewmodels;

public class LoadingViewModel : ObservableObject, IClientCallback
{

    public NavigationStore NavigationStore { get; set; }

    private string _image;
    public string Image
    {
        get => _image;
        set
        {
            _image = value;
            OnPropertyChanged();
        }
    }

    private string _message = "Een ogenblik geduld, we laden alles in...";
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }


    public LoadingViewModel(NavigationStore navigationStore, string user)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        _image = "/resources/" + user + ".gif";
    }

    public void OnCallback(State state, string value = "")
    {
        switch (state)
        {
            case Success:
                NavigationStore.CurrentViewModel = new SuccessViewModel(NavigationStore);
                break;
            case Error:
                Message = value;
                Image = "resources/warning.png";
                break;
        }
    }
}