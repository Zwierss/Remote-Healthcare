using DoctorApplication.stores;
using DoctorLogic;
using MvvmHelpers;

namespace DoctorApplication.viewmodels;

public class ClientViewModel : ObservableObject, IWindow
{
    public NavigationStore NavigationStore { get; }
    public string UserId { get; }

    public ClientViewModel(NavigationStore navigationStore, string userId)
    {
        UserId = userId;
        NavigationStore = navigationStore;
        NavigationStore.Client.ViewModel = this;
    }

    public void OnChangedValues(State state, string value = "")
    {
        throw new System.NotImplementedException();
    }
}