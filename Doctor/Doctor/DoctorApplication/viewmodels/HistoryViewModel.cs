using System.Collections.Generic;
using System.Linq;
using DoctorApplication.commands.HistoryView;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using MvvmHelpers.Commands;
using static DoctorLogic.State;
using ICommand = System.Windows.Input.ICommand;

namespace DoctorApplication.viewmodels;

public class HistoryViewModel: ObservableObject, IWindow
{
    public string SelectedItem { get; set; }

    public NavigationStore NavigationStore { get; }
    
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

    public ICommand GoBack { get; }
    public ICommand Select { get; }

    public HistoryViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
        NavigationStore.Client.GetOfflineClients();
        GoBack = new GoBackHistoryCommand(this);
        Select = new SelectHistoryCommand(this);
    }

    public void OnChangedValues(State state, string[] args = null)
    {
        switch (state)
        {
            case Data:
                Clients = args!.ToList();
                break;
        }
    }
}