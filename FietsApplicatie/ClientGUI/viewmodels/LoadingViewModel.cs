using System;
using ClientApplication;
using DoctorApplication.stores;
using MvvmHelpers;
using static ClientApplication.State;

namespace ClientGUI.viewmodels;

public class LoadingViewModel : ObservableObject, IClientCallback
{

    public NavigationStore NavigationStore { get; set; }

    public LoadingViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
    }

    public void OnCallback(State state, string value = "")
    {
        switch (state)
        {
            case Success:
                NavigationStore.CurrentViewModel = new SuccessViewModel(NavigationStore);
                break;
        }
    }
}