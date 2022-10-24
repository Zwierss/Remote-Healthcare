using System;
using DoctorLogic;
using MvvmHelpers;

namespace DoctorApplication.stores;

public class NavigationStore
{
    public DoctorClient Client { get; set; }

    public NavigationStore(DoctorClient client)
    {
        Client = client;
    }

    public event Action CurrentViewModelChanged;

    private ObservableObject _currentViewModel;
    
    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}