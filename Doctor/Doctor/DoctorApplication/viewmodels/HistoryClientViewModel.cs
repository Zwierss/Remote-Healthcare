using System.Collections.Generic;
using System.Linq;
using DoctorApplication.commands.HistoryClientView;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels;

public class HistoryClientViewModel : ObservableObject, IWindow
{

    public string Client { get; }
    public string SelectedItem { get; set; }
    public NavigationStore NavigationStore { get; }

    private List<string> _sessions;
    public List<string> Sessions
    {
        get { return _sessions ??= new List<string>(); }
        set
        {
            _sessions = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand GoBack { get; }
    public ICommand Select { get; set; }

    public HistoryClientViewModel(NavigationStore navigationStore, string client)
    {
        NavigationStore = navigationStore;
        Client = client;
        NavigationStore.Client.ViewModel = this;
        NavigationStore.Client.GetSessions(client);
        GoBack = new GoBackHistoryClientCommand(this);
        Select = new SelectHistoryClientCommand(this);
    }

    public void OnChangedValues(State state, string[] args = null)
    {
        switch (state)
        {
            case Data:
                Sessions = args!.ToList();
                break;
        }
    }
}