using System;
using Ssl.Net;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DtlsClient client = new DtlsClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 33903));
            Task.Run(async () =>
            {
                while (true)
                {
                    var buffer = new byte[1024];
                    var count = await client.ReceiveAsync(buffer, 0, buffer.Length);
                }
            }).Wait();

        }
    }
}