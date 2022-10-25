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
    public double[] Times { get; set; }
    public double[] Speeds { get; set; }
    public double[] Beats { get; set; }

    public NavigationStore NavigationStore { get; }
    public string Client { get; }
    public string Item { get; }
    public Func<double, string> Formatter { get; set; }

    public ChartViewModel ViewModel { get; set; }

    public ViewHistoryViewModel(NavigationStore navigationStore, string client, string item, double[] speeds, double[] times, double[] beats)
    {
        Times = times;
        Speeds = speeds;
        Beats = beats;
        
        NavigationStore = navigationStore;
        Client = client;
        Item = item;
        NavigationStore.Client.ViewModel = this;

        ViewModel = new ChartViewModel();
    }

    public void OnChangedValues(State state, string[] args = null)
    {
        
    }
}