using DoctorLogic;

DoctorClient client = new DoctorClient("Admin", "Admin", "localhost", 6666);
await client.SetupConnection();

while (true)
{
    Thread.Sleep(10);
}