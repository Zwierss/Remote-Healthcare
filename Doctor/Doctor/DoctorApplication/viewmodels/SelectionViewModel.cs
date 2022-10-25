#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using DoctorApplication.commands;
using DoctorApplication.commands.SelectionView;
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
    public ICommand GoBack { get; }
    public ICommand ViewHistoric { get; }

    public SelectionViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
        NavigationStore.Client.GetClients();
        Selection = new SelectSelectionCommand(this);
        GoBack = new GoBackSelectionCommand(this);
        ViewHistoric = new ViewHistoricCommand(this);
    }

    public void OnChangedValues(State state, string[]? args = null)
    {
        switch (state)
        {
            case Store:
                Clients = args!.ToList();
                break;
            case Replace:
                Clients.Clear();
                break;
            case Error:
                break;
            case Success:
                NavigationStore.CurrentViewModel = new ClientViewModel(NavigationStore, SelectedClient);
                break;
        }
    }
}