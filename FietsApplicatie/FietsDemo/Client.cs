using Avans.TI.BLE;
using System.Threading.Tasks;

namespace FietsDemo
{
    public class Client
    {


        private readonly BLE _ble;

        public Client()
        {
            _ble = new BLE();
        }

        public async Task<bool> MakeConnection()
        {
            return true;
        }
    }
}