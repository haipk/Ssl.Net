using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ssl.Net
{
    public class TlsListener : ISslListener
    {
        public IPEndPoint LocalEP => throw new NotImplementedException();

        public TlsClient Accept()
        {

            throw new NotImplementedException();
        }

        public Task<ISslClient> AcceptAsync()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        ISslClient ISslListener.Accept()
        {
            throw new NotImplementedException();
        }
    }
}
