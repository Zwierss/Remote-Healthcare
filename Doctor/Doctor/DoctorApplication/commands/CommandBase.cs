using System;
using System.Windows.Input;

namespace DoctorApplication.commands;

public abstract class CommandBase : ICommand
{
    /* Declaring an event that can be subscribed to. */
    public event EventHandler CanExecuteChanged;
    
    /// <summary>
    /// The CanExecute function is a virtual function that returns true
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public virtual bool CanExecute(object parameter) => true;

    /// <summary>
    /// The Execute function is called when the command is invoked
    /// </summary>
    /// <param name="parameter">The parameter passed to the command.</param>
    public abstract void Execute(object parameter);

    /// <summary>
    /// If the CanExecuteChanged event is not null, invoke it
    /// </summary>
    protected void OnCanExcecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}