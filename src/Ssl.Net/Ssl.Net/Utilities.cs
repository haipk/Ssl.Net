using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.IO.Pem;

namespace Ssl.Net
{
    public static class Utilities
    {
         
        public static TlsAgreementCredentials LoadAgreementCredentials(TlsContext context,
            string[] certResources, string keyResource)
        {
            Certificate certificate = LoadCertificateChain(certResources);
            AsymmetricKeyParameter privateKey = LoadPrivateKeyResource(keyResource);

            return new DefaultTlsAgreementCredentials(certificate, privateKey);
        }

        public static TlsEncryptionCredentials LoadEncryptionCredentials(TlsContext context,
            string[] certResources, string keyResource)
        {
            Certificate certificate = LoadCertificateChain(certResources);
            AsymmetricKeyParameter privateKey = LoadPrivateKeyResource(keyResource);

            return new DefaultTlsEncryptionCredentials(context, certificate, privateKey);
        }

        public static TlsSignerCredentials LoadSignerCredentials(TlsContext context, string[] certResources,
            string keyResource, SignatureAndHashAlgorithm signatureAndHashAlgorithm)
        {
            Certificate certificate = LoadCertificateChain(certResources);
            AsymmetricKeyParameter privateKey = LoadPrivateKeyResource(keyResource);

            return new DefaultTlsSignerCredentials(context, certificate, privateKey, signatureAndHashAlgorithm);
        }

        public static TlsSignerCredentials LoadSignerCredentials(TlsContext context, IList supportedSignatureAlgorithms,
            byte signatureAlgorithm, string certResource, string keyResource)
        {
            /*
             * TODO Note that this code fails to provide default value for the client supported
             * algorithms if it wasn't sent.
             */

            SignatureAndHashAlgorithm signatureAndHashAlgorithm = null;
            if (supportedSignatureAlgorithms != null)
            {
                foreach (SignatureAndHashAlgorithm alg in supportedSignatureAlgorithms)
                {
                    if (alg.Signature == signatureAlgorithm)
                    {
                        signatureAndHashAlgorithm = alg;
                        break;
                    }
                }

                if (signatureAndHashAlgorithm == null)
                    return null;
            }

            return LoadSignerCredentials(context, new String[] { certResource },
                keyResource, signatureAndHashAlgorithm);
        }

        public static Certificate LoadCertificateChain(string[] resources)
        {
            X509CertificateStructure[] chain = new X509CertificateStructure[resources.Length];
            for (int i = 0; i < resources.Length; ++i)
            {
                chain[i] = LoadCertificateResource(resources[i]);
            }
            return new Certificate(chain);
        }

        public static X509CertificateStructure LoadCertificateResource(string resource)
        {
            PemObject pem = LoadPemResource(resource);
            if (pem.Type.EndsWith("CERTIFICATE"))
            {
                return X509CertificateStructure.GetInstance(pem.Content);
            }
            throw new ArgumentException("doesn't specify a valid certificate", "resource");
        }

        public static AsymmetricKeyParameter LoadPrivateKeyResource(string resource)
        {
            PemObject pem = LoadPemResource(resource);
            if (pem.Type.EndsWith("RSA PRIVATE KEY"))
            {
                RsaPrivateKeyStructure rsa = RsaPrivateKeyStructure.GetInstance(pem.Content);
                return new RsaPrivateCrtKeyParameters(rsa.Modulus, rsa.PublicExponent,
                    rsa.PrivateExponent, rsa.Prime1, rsa.Prime2, rsa.Exponent1,
                    rsa.Exponent2, rsa.Coefficient);
            }
            if (pem.Type.EndsWith("PRIVATE KEY"))
            {
                return PrivateKeyFactory.CreateKey(pem.Content);
            }
            throw new ArgumentException("doesn't specify a valid private key", "resource");
        }

        public static PemObject LoadPemResource(string resource)
        {
            using (Stream s = File.Open(resource, FileMode.Open))
            {
                PemReader p = new PemReader(new StreamReader(s));
                PemObject o = p.ReadPemObject();
                return o;
            }
        }
    }
}
