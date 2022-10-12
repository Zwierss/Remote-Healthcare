using ClientApplication;

string? username = Console.ReadLine();
string? bikeId = Console.ReadLine();

Client client = new(Environment.MachineName,"01140", "", 0);
await client.SetupConnection();