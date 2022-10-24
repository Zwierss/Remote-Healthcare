using System.Windows.Forms;
using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.SelectionView;

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
        _view.NavigationStore.Client.Subscribe(_view.NavigationStore.Client.Uuid,_view.SelectedClient);
    }
}