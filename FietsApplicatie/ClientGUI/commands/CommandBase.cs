using System;
using System.Windows.Input;

namespace ClientGUI.commands;

public abstract class CommandBase : ICommand
{
    public event EventHandler CanExecuteChanged;
    
    public virtual bool CanExecute(object parameter) => true;

    public abstract void Execute(object parameter);

    protected void OnCanExcecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}