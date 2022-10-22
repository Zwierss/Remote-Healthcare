using DoctorLogic;

DoctorClient client = new DoctorClient("10", "localhost", 6666);
await client.SetupConnection();

while (true)
{
    Thread.Sleep(10);
}