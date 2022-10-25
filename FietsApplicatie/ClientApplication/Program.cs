using ClientApplication;

namespace ClientApplication;

public class Program
{
    public static void Main(string[] args)
    {
        
        //Client client = new(Environment.MachineName,"01140", "localhost", 6666);
        Client client = new(Environment.MachineName,"01140", "192.168.43.137", 15243);
        client.SetupConnection();

        while (true)
        {
            Thread.Sleep(10);
        }
    }
}

// string? username = Console.ReadLine();
// string? bikeId = Console.ReadLine();

