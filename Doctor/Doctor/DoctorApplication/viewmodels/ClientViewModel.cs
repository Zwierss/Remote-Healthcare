using System;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;

namespace DoctorApplication.viewmodels;

public class ClientViewModel : ObservableObject, IWindow
{
    public NavigationStore NavigationStore { get; }
    public string UserId { get; }
    public string DoctorMsg { get; set; }
    public string Resistance { get; set; }

    private string _onlineStr = "Online";
    public string OnlineStr
    {
        get => _onlineStr;
        set
        {
            _onlineStr = value;
            OnPropertyChanged();
        }
    }

    private string _onlineSrc = "resources/online.png";
    public string OnlineSrc
    {
        get => _onlineSrc;
        set
        {
            _onlineSrc = value;
            OnPropertyChanged();
        }
    }

    private string _sessionBtn = "Start sessie";
    public string SessionBtn
    {
        get => _sessionBtn;
        set
        {
            _sessionBtn = value;
            OnPropertyChanged();
        }
    }

    private string _speed = "--";
    public string Speed
    {
        get => _speed;
        set
        {
            _speed = value;
            OnPropertyChanged();
        }
    }

    private string _distance = "--";
    public string Distance
    {
        get => _distance;
        set
        {
            _distance = value;
            OnPropertyChanged();
        }
    }

    private string _speedAvg = "--";
    public string SpeedAvg
    {
        get => _speedAvg;
        set
        {
            _speedAvg = value;
            OnPropertyChanged();
        }
    }

    private string _heartbeat = "--";
    public string Heartbeat
    {
        get => _heartbeat;
        set
        {
            _heartbeat = value;
            OnPropertyChanged();
        }
    }

    private string _time = "--";
    public string Time
    {
        get => _time;
        set
        {
            _time = value;
            OnPropertyChanged();
        }
    }

    public ClientViewModel(NavigationStore navigationStore, string userId)
    {
        UserId = userId;
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
    }

    public void OnChangedResistance(int resistance)
    {
        Console.WriteLine(resistance);
    }

    public void OnChangedValues(State state, string value = "")
    {
        
    }
}