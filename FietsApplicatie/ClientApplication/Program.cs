using ClientApplication;

namespace ClientApplication;

public static class Program
{
    public static void Main(string[] args)
    {
        
        Client client = new();
        client.SetupConnection("Test","Test", "localhost", 6666);

        while (true)
        {
            Thread.Sleep(10);
        }
    }
}

// string? username = Console.ReadLine();
// string? bikeId = Console.ReadLine();

