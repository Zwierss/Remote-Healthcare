using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Forms;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels;

public class SelectionViewModel : ObservableObject, IWindow
{

    public NavigationStore NavigationStore { get; set; }

    private List<string> _clients;
    public List<string> Clients
    {
        get { return _clients ??= new List<string>(); }
        set
        {
            _clients = value;
            OnPropertyChanged();
        }
    }
    
    public SelectionViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
        NavigationStore.Client.GetClients();
    }

    public void OnChangedValues(State state, string value = "")
    {
        switch (state)
        {
            case Store:
                List<string> clients = new List<string>(Clients){value};
                Clients = clients;
                break;
        }
    }
}