using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;

namespace Ssl.Net
{
    internal class TlsServerImpl : DefaultTlsServer
    {

        private ProtocolVersion _version;

        public TlsServerImpl( ProtocolVersion version)
        {
            mServerVersion = version;

        }

        protected override int[] GetCipherSuites()
        {
            return Arrays.Concatenate(base.GetCipherSuites(),
                new int[] { });
        }
         

    }
}
