#nullable enable
using System;
using System.Linq;
using DoctorApplication.commands;
using DoctorApplication.commands.ClientView;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels;

public class ClientViewModel : ObservableObject, IWindow
{
    public NavigationStore NavigationStore { get; }
    public string UserId { get; }
    public string DoctorMsg { get; set; }
    public bool SessionIsActive { get; set; }
    public bool IsOnline { get; set; }

    private string _errorMessage = "";
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

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

    private string _heartbeatAvg = "--";
    public string HeartbeatAvg
    {
        get => _heartbeatAvg;
        set
        {
            _heartbeatAvg = value;
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

    public ICommand EmergencyStop { get; }
    public ICommand SessionC { get; }
    public ICommand SendMessage { get; }
    public ICommand GoBack { get; }

    public ClientViewModel(NavigationStore navigationStore, string userId)
    {
        IsOnline = true;
        UserId = userId;
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
        EmergencyStop = new EmergencyStopCommand(this);
        SessionC = new SessionCommand(this);
        SendMessage = new SendMesageCommand(this);
        GoBack = new GoBackClientCommand(this);
        SessionIsActive = false;
    }

    public void OnChangedResistance(int resistance)
    {
        if (!IsOnline) return;
        NavigationStore.Client.ChangeResistance(UserId, resistance);
    }

    public void OnChangedValues(State state, string[]? args = null)
    {
        switch (state)
        {
            case Data:
                Speed = args![0];
                Heartbeat = args[1];
                SpeedAvg = args[2];
                HeartbeatAvg = args[3];
                Distance = args[4];
                Time = args[5];
                break;
            case Store:
                if (args!.ToList().Contains(UserId))
                {
                    OnlineSrc = "resources/online.png";
                    OnlineStr = "Online";
                    IsOnline = true;
                    ErrorMessage = "";
                }
                else
                {
                    IsOnline = false;
                    OnlineSrc = "resources/offline.png";
                    OnlineStr = "Offline";
                    SessionC.Execute(this);
                }

                break;
        }
    }
}