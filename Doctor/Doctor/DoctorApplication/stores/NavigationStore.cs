using System;
using DoctorLogic;
using MvvmHelpers;

namespace DoctorApplication.stores;

public class NavigationStore
{
    public DoctorClient Client { get; }

    /* This is a constructor. It is called when an instance of the class is created. */
    public NavigationStore(DoctorClient client)
    {
        Client = client;
    }

    public event Action CurrentViewModelChanged;

    private ObservableObject _currentViewModel;
    
    /* A property that is used to set the current view model. */
    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    /// <summary>
    /// "When the CurrentViewModel property changes, invoke the CurrentViewModelChanged event."
    /// 
    /// The CurrentViewModelChanged event is a delegate that is defined in the ViewModelLocator class
    /// </summary>
    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}