using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ssl.Net
{
    internal class TlsClientImpl : DefaultTlsClient
    {
        public override ProtocolVersion MinimumVersion => ProtocolVersion.DTLSv12;
        public override ProtocolVersion ClientVersion => ProtocolVersion.DTLSv12;
        public override int[] GetCipherSuites()
        {
            return new[] { CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_256_CCM };
        }
        public override TlsAuthentication GetAuthentication()
        {
            return new TlsAuthenticationImpl();
        }
    }
}
