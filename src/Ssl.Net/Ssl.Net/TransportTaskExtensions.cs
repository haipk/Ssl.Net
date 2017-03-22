using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ssl.Net
{
    internal static class TransportTaskExtensions
    {
        public static async Task<int> ReceiveAsync(this DatagramTransport transport, byte[] buf, int off, int len)
        {
            SpinWait spinWait = new SpinWait();
            while (true)
            {
                int count = transport.Receive(buf, off, len, 0);
                if (count == -1 )
                {
                    spinWait.SpinOnce();
                    await Task.Yield();
                }
                else
                {
                    Console.WriteLine(count);
                    return count;
                }
            }
        }

        public static Task SendAsync(this DatagramTransport transport, byte[] buf, int off, int len)
        {
            return Task.Run(() => transport.Send(buf, off, len));
        }
    }
}
