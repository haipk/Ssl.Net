using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ssl.Net
{
    public class DtlsClient : ISslClient
    {
        public bool Connected => throw new NotImplementedException();

        public IPEndPoint LocalEp => throw new NotImplementedException();

        public IPEndPoint RemoteEP => throw new NotImplementedException();

        internal DtlsClient(DtlsTransport transport)
        {

        }
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Connect(IPEndPoint remoteEP)
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync(IPEndPoint remoteEP)
        {
            throw new NotImplementedException();
        }

        public int Receive(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public Task<int> ReceiveAsync(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public int Send(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendAsync(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
