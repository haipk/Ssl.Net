using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Ssl.Net
{
    internal class UdpTransport : DatagramTransport
    {
        private Socket _socket;

        public UdpTransport(Socket socket)
        {
            _socket = socket;
        }

        public void Close()
        {
        }

        public int GetReceiveLimit()
        {
            return 1024 * 4;
        }

        public int GetSendLimit()
        {
            return 1024 * 4;
        }

        public int Receive(byte[] buf, int off, int len, int waitMillis)
        {

            if (_socket.Connected)
            {
                if (waitMillis == 0 && _socket.Available == 0)
                {
                    return -1;
                }
                if (SpinWait.SpinUntil(() => _socket.Available > 0, waitMillis))
                {
                    return _socket.Receive(buf, off, len, SocketFlags.None);
                }
                else
                {
                    if (waitMillis == 60000) // 1 min
                    {
                        throw new TimeoutException();
                    }
                    return -1;
                }
            }
            throw new NotImplementedException();
        }

        public void Send(byte[] buf, int off, int len)
        {
            if (_socket.Connected)
            {
                _socket.Send(buf, off, len, SocketFlags.None);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
