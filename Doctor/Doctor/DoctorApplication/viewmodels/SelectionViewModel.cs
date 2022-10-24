using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Forms;
using DoctorApplication.commands;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using static DoctorLogic.State;
using ICommand = System.Windows.Input.ICommand;

namespace DoctorApplication.viewmodels;

public class SelectionViewModel : ObservableObject, IWindow
{
    public string SelectedClient { get; set; }

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

    public ICommand Selection { get; }

    public SelectionViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
        NavigationStore.Client.GetClients();
        Selection = new SelectCommand(this);
    }

    public void OnChangedValues(State state, string value = "")
    {
        switch (state)
        {
            case Store:
                List<string> clients = new List<string>(Clients){value};
                Clients = clients;
                break;
            case Replace:
                Clients.Clear();
                break;
        }
    }
}