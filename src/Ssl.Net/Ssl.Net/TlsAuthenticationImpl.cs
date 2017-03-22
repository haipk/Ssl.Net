using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ssl.Net
{
    internal class TlsAuthenticationImpl : TlsAuthentication
    {
        public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest)
        {
            throw new NotImplementedException();
        }

        public void NotifyServerCertificate(Certificate serverCertificate)
        {
            throw new NotImplementedException();
        }
    }
}
