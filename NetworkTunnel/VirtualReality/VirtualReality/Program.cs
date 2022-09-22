namespace VirtualReality;

public class Program
{
    public static void Main(string[] args)
    {

        Client client = new Client();
        ClientApplication  clientApplication = new ClientApplication();
#pragma warning disable CS4014
        //client.StartConnection();
#pragma warning restore CS4014

        int count = 0;
        bool onlyOnce = true;
        
        while (true)
        {

            Thread.Sleep(10);
        }
    }
}