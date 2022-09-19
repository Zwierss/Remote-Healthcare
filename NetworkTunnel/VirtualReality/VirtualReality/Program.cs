using System.Collections.Specialized;

namespace VirtualReality;

public class Program
{
    public static void Main(string[] args)
    {
#pragma warning disable CS4014
        Client.GetInstance().StartConnection();
#pragma warning restore CS4014

        int count = 0;
        
        while (true)
        {
            if (count == 10)
            {
                count = 0;
                Client.GetInstance().SendData(@"{""id"": ""session/list""}");
            }
            Thread.Sleep(100);
            count++;
        }
    }
}