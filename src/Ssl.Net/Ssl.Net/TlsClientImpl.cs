using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ssl.Net
{
    internal class TlsClientImpl : DefaultTlsClient
    {

        public TlsClientImpl(ProtocolVersion version)
        {
        }

        public override TlsAuthentication GetAuthentication()
        {
            throw new NotImplementedException();
        }
    }
}
