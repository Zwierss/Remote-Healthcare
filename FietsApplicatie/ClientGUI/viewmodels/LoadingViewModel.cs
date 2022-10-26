using System;
using System.Security;
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
    /* A property that is used to set the image of the loading screen. */
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
    /* This is a property that is used to set the message of the loading screen. */
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
    /* This is a property that is used to set the countdown of the loading screen. */
    public string Countdown 
    {
        get => _countdown;
        set 
        {
            _countdown = value;
            OnPropertyChanged();
        }
    }

    private string _user;
    private string _pass;
    private string _ip;
    private int _port;
    private string _bike;
    private bool _sim;
    private Thread _threadCounter;

    public ICommand GoBack { get; }

    /* This is the constructor of the LoadingViewModel. It is used to set the properties of the LoadingViewModel. */
    public LoadingViewModel(NavigationStore navigationStore, string user, string pass, string ip, int port, string bike, bool sim)
    {
        _user = user;
        _pass = pass;
        _ip = ip;
        _port = port;
        _bike = bike;
        _sim = sim;

        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        if (user.ToLower() == "rick") 
        {
            _image = "/resources/rick.gif";
        }

        
        GoBack = new GoBackCommand(this);
        new Thread(SetupConnection).Start();
        _threadCounter = new Thread(StartCountdown);
    }

    /// <summary>
    /// It counts down from 5 to 0 and then navigates to the BeginViewModel
    /// </summary>
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

    /// <summary>
    /// > This function sets up the connection to the server
    /// </summary>
    public void SetupConnection() 
    {
        NavigationStore.Client.SetupConnection(_user, _pass, _ip, _port, _bike, _sim);
    }

    /// <summary>
    /// If the state is Success, then navigate to the SuccessViewModel. If the state is Error, then set the Message and
    /// Image properties and start the thread counter
    /// </summary>
    /// <param name="State">This is an enum that you can define yourself. It's used to determine what the callback should
    /// do.</param>
    /// <param name="value">The value of the message to be displayed.</param>
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