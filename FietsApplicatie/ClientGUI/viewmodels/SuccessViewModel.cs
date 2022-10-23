using System.Windows.Input;
using ClientApplication;
using ClientGUI.commands;
using DoctorApplication.stores;
using MvvmHelpers;
using ICommand = System.Windows.Input.ICommand;

namespace ClientGUI.viewmodels;

public class SuccessViewModel : ObservableObject, IClientCallback
{

    public NavigationStore NavigationStore { get; set; }
    public ICommand Stop{ get; set; }

    public SuccessViewModel(NavigationStore navigationStore)
    {
        NavigationStore = navigationStore;
        NavigationStore.Client.Callback = this;
        Stop = new StopCommand(this);
    }

    public void OnCallback(State state, string value = "")
    {
        
    }
}