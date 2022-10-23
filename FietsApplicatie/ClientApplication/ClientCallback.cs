namespace ClientApplication;

public interface IClientCallback
{
    void OnCallback(State state, string value = "");
}