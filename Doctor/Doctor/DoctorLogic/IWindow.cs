namespace DoctorLogic;

public interface IWindow
{
    void OnChangedValues(State state, string[]? args = null);
}