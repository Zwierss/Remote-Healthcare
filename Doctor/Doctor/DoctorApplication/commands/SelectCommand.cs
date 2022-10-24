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
        
    }
}