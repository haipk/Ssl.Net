using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ssl.Net
{
    internal static class TransportTaskExtensions
    {
        public const int WaitReceive = -3; // state is waitting receive
        public const int WaitSend = -4; // state is watting receive
        public const int Nonblock = 0;
        public static async Task<int> ReceiveAsync(this DatagramTransport transport, byte[] buf, int off, int len)
        {
            while (true)
            {
                int count = transport.Receive(buf, off, len, Nonblock);
                if (count == WaitReceive || count == WaitSend)
                {
                    await Task.Yield();
                }
                else
                {
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
