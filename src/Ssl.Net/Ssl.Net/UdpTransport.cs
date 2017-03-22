using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Ssl.Net
{
    internal class UdpTransport : DatagramTransport
    {
        private Queue<byte[]> _receiveQueue;
        private Socket _socket;

        public UdpTransport(Socket socket)
        {
            _socket = socket;
        }
        public UdpTransport(Socket socket, Queue<byte[]> receiveQueue)
        {
            _socket = socket;
            _receiveQueue = receiveQueue;
        }

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
                if (_receiveQueue.Count > 0)
                {
                    byte[] data = _receiveQueue.Dequeue();
                    if (data.Length > len)
                    {
                        throw new NotSupportedException();
                    }
                    Buffer.BlockCopy(data, 0, buf, off, data.Length);
                }
                else
                {
                    return TransportTaskExtensions.Wait;
                }

            }
            else // blocking
            {
                if (SpinWait.SpinUntil(() => _receiveQueue.Count > 0, waitMillis))
                {
                    if (_receiveQueue.Count > 0)
                    {
                        byte[] data = _receiveQueue.Dequeue();
                        if (data.Length > len)
                        {
                            throw new NotSupportedException();
                        }
                        Buffer.BlockCopy(data, 0, buf, off, data.Length);
                    };
                }
                else
                {
                    throw new TimeoutException();
                }

            }
            throw new NotImplementedException();
        }

        public void Send(byte[] buf, int off, int len)
        {
            throw new NotImplementedException();
        }
    }
}
