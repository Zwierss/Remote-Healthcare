using DoctorLogic;

DoctorClient client = new DoctorClient();
client.SetupConnection("Admin", "Admin", "localhost", 6666);

while (true)
{
    Thread.Sleep(10);
}