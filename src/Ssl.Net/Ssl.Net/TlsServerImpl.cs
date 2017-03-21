using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace Ssl.Net
{
    internal class TlsServerImpl : DefaultTlsServer
    {
        private X509Certificate _certifcate;

        public TlsServerImpl(X509Certificate certifcate, ProtocolVersion version)
        {
            _certifcate = certifcate;
        }
    }
}
