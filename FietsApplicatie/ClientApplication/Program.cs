using ClientApplication;

namespace ClientApplication;

public static class Program
{
    public static async Task Main(string[] args)
    {
        
        Client client = new(Environment.MachineName,"01140", "localhost", 6666);
        await client.SetupConnection();

        while (true)
        {
            Thread.Sleep(10);
        }
    }
}

// string? username = Console.ReadLine();
// string? bikeId = Console.ReadLine();

