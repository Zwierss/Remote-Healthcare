using System;
using DoctorApplication.viewmodels;

namespace DoctorApplication.commands;

public class SelectCommand : CommandBase
{
    private SelectionViewModel _view;

    public SelectCommand(SelectionViewModel view)
    {
        _view = view;
    }

    public override void Execute(object parameter)
    {
        string client = _view.SelectedClient;
        if (string.IsNullOrEmpty(client)) return;
        _view.NavigationStore.CurrentViewModel = new ClientViewModel(_view.NavigationStore, client);
    }
}