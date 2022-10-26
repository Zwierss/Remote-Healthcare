using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Markup;
using ClientApplication;
using ClientGUI.commands;
using DoctorApplication.stores;
using MvvmHelpers;
using static ClientApplication.State;
using ICommand = System.Windows.Input.ICommand;

namespace ClientGUI.viewmodels;

public class LoadingViewModel : ObservableObject, IClientCallback
{

    public NavigationStore NavigationStore { get; set; }

    private string _image = "/resources/load.gif";
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

    private string _countdown = "";
    public string Countdown 
    {
        get => _countdown;
        set 
        {
            _countdown = value;
            OnPropertyChanged();
        }
    }

    private Thread _threadCounter;

    public ICommand GoBack { get; }

    public LoadingViewModel(NavigationStore navigationStore, string user, string pass, string ip, int port, string bike, bool sim)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        if (user.ToLower() == "rick") 
        {
            _image = "/resources/rick.gif";
        }

        NavigationStore.Client.SetupConnection(user, pass, ip, port, bike, sim);
        GoBack = new GoBackCommand(this);
        _threadCounter = new Thread(StartCountdown);
    }

    public void StartCountdown() 
    {
        for (int i = 5; i >= 0; i--) 
        {
            Countdown = "Sluit in " + i;
            Thread.Sleep(1000);
        }
        NavigationStore.CurrentViewModel = new BeginViewModel(NavigationStore);
        NavigationStore.Client.Stop(true);
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
                _threadCounter.Start();
                break;
        }
    }
}