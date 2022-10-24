using DoctorApplication.stores;
using MvvmHelpers;

namespace ClientGUI.viewmodels;

public class MainViewModel : ObservableObject
{

    private readonly NavigationStore _navigationStore;

    public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}