using DoctorApplication.stores;
using MvvmHelpers;

namespace ClientGUI.viewmodels;

public class MainViewModel : ObservableObject
{

    private readonly NavigationStore _navigationStore;

    public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;

    /* Subscribing to the event CurrentViewModelChanged. */
    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    /// <summary>
    /// When the CurrentViewModel property changes, call the OnPropertyChanged function and pass in the name of the property
    /// that changed.
    /// </summary>
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}