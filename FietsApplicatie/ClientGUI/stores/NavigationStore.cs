using System;
using ClientApplication;
using MvvmHelpers;

namespace DoctorApplication.stores;

public class NavigationStore
{
    public event Action CurrentViewModelChanged;

    private ObservableObject _currentViewModel;
    public Client Client { get; set; }

    public NavigationStore(Client client)
    {
        Client = client;
    }

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