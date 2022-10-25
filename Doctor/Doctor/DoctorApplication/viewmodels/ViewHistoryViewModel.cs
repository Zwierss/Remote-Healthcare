using System.Collections.Generic;
using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels;

public class ViewHistoryViewModel : ObservableObject, IWindow
{

    public List<string[]> SessionData { get; }
    public NavigationStore NavigationStore { get; }
    public string Client { get; }
    public string Item { get; }

    public ViewHistoryViewModel(NavigationStore navigationStore, string client, string item)
    {
        SessionData = new List<string[]>();
        NavigationStore = navigationStore;
        Client = client;
        Item = item;
        NavigationStore.Client.ViewModel = this;
        NavigationStore.Client.GetSessionData(client, item);
    }

    public void OnChangedValues(State state, string[] args = null)
    {
        switch (state)
        {
            case Data:
                SessionData.Add(args);
                break;
        }
    }
}