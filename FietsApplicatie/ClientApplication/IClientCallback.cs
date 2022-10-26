namespace ClientApplication;

public interface IClientCallback
{
    /// <summary>
    /// The function takes two parameters, a State and a string. The State is an enum that can be one of three values:
    /// Started, Stopped, or Error. The string is an optional parameter that can be used to pass a message to the callback
    /// function.
    /// </summary>
    /// <param name="State">The state of the callback.</param>
    /// <param name="value">The value of the callback.</param>
    void OnCallback(State state, string value = "");
}