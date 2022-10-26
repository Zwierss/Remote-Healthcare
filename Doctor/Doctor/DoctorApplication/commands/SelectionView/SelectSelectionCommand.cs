using System.Windows.Forms;
using DoctorApplication.viewmodels;

namespace DoctorApplication.commands.SelectionView;

public class SelectSelectionCommand : CommandBase
{
    private SelectionViewModel _view;

    /* A constructor. */
    public SelectSelectionCommand(SelectionViewModel view)
    {
        _view = view;
    }

    /// <summary>
    /// The function subscribes to the selected client's UUID
    /// </summary>
    /// <param name="parameter">The parameter passed to the command when it was executed.</param>
    /// <returns>
    /// The client is being returned.
    /// </returns>
    public override void Execute(object parameter)
    {
        string client = _view.SelectedClient;
        if (string.IsNullOrEmpty(client)) return;
        _view.NavigationStore.Client.Subscribe(_view.NavigationStore.Client.Uuid,_view.SelectedClient);
    }
}