namespace ClientApplication;

public interface ClientCallback
{
    void OnCallback(State state, string value = "");
}