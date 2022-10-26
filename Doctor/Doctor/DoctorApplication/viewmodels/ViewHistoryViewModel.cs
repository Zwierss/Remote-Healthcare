using System;
using System.Collections.Generic;
using System.Linq;
using DoctorApplication.stores;
using DoctorLogic;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using MvvmHelpers;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels;

public class ViewHistoryViewModel : ObservableObject, IWindow
{
    private double[] _speedArray;
    private double[] _beatArray;

    public string SpeedAvg { get; set; }
    public string BpmAvg { get; set; }

    public NavigationStore NavigationStore { get; set; }
    public string Client { get; set; }
    public string Item { get; set; }

    private ChartValues<double> _speeds;
    public ChartValues<double> Speeds 
    {
        get => _speeds;
        set 
        {
            _speeds = value;
            OnPropertyChanged();
        }
    }

    private ChartValues<double> _beats;
    public ChartValues<double> Beats 
    {
        get => _beats;
        set
        {
            _beats = value;
            OnPropertyChanged();
        }
    }

    public ViewHistoryViewModel(NavigationStore navigationStore, string client, string item, double[] speeds, double[] times, double[] beats)
    {
        _speedArray = speeds;
        _beatArray = beats;

        double speedMax = 0;
        foreach(double speed in speeds) 
        { 
            speedMax += speed;
        }
        SpeedAvg = Math.Round((speedMax / speeds.Length), 1, MidpointRounding.AwayFromZero).ToString();

        int bpmMax = 0;
        foreach (double bpm in beats)
        {
            bpmMax += (int)bpm;
        }
        BpmAvg = (bpmMax / beats.Length).ToString();

        NavigationStore = navigationStore;
        Client = client;
        Item = item;
        NavigationStore.Client.ViewModel = this;

        Speeds = _speedArray.AsEnumerable().AsChartValues<double>();
        Beats = _beatArray.AsEnumerable().AsChartValues<double>();
    }


    public void OnChangedValues(State state, string[] args = null)
    {
        
    }
}