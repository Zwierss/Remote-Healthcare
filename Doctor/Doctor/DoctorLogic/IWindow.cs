namespace DoctorLogic;

public interface IWindow
{
    /// <summary>
    /// > This function is called when the values of the state change
    /// </summary>
    /// <param name="State">The state of the event.</param>
    /// <param name="args">The arguments that were passed to the command.</param>
    void OnChangedValues(State state, string[]? args = null);
}