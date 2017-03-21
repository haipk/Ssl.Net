using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ssl.Net
{
    internal class UdpTransport : DatagramTransport
    {
        private Stream _inputStream;
        private Stream _outputStream;
        public void Close()
        {
            throw new NotImplementedException();
        }

        public int GetReceiveLimit()
        {
            throw new NotImplementedException();
        }

        public int GetSendLimit()
        {
            throw new NotImplementedException();
        }

        public int Receive(byte[] buf, int off, int len, int waitMillis)
        {
            if (waitMillis == TransportTaskExtensions.Nonblock) // non blocking
            {
                if (_inputStream.Length > 0)
                {
                    return _inputStream.Read(buf, off, len);
                }
                else
                {
                    return TransportTaskExtensions.WaitReceive;
                }

            }
            else // blocking
            {
                return _inputStream.Read(buf, off, len);
            }
            throw new NotImplementedException();
        }

        public void Send(byte[] buf, int off, int len)
        {
            throw new NotImplementedException();
        }
    }
}
