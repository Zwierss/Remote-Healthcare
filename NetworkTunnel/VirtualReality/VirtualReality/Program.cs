namespace VirtualReality;

public static class Program
{
    public static void Main(string[] args)
    {

        Client client = new Client();
#pragma warning disable CS4014
        client.StartConnection();
#pragma warning restore CS4014

        while (true)
        {

            Thread.Sleep(10);
        }
    }
}