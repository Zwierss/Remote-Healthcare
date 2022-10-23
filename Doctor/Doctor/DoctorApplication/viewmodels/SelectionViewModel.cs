using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Forms;
using DoctorLogic;
using MvvmHelpers;
using static DoctorLogic.State;

namespace DoctorApplication.viewmodels;

public class SelectionViewModel : ObservableObject, IWindow
{

    private List<string> _clients;
    public List<string> Clients
    {
        get { return _clients ??= new List<string>(); }
        set
        {
            _clients = value;
            OnPropertyChanged();
        }
    }

    public DoctorClient Client { get; set; }

    public SelectionViewModel(DoctorClient client)
    {
        Client = client;
        Client.ViewModel = this;
        Client.GetClients();
    }

    public void OnChangedValues(State state, string value = "")
    {
        switch (state)
        {
            case Store:
                List<string> clients = new List<string>(Clients){value};
                Clients = clients;
                break;
        }
    }
}