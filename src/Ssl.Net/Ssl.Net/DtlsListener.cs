using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ssl.Net
{
    public class DtlsListener : ISslListener
    {
        private Socket _socket;
        private ConcurrentQueue<UdpTransport> _acceptQueue;
        private X509Certificate _certifcate;
        public IPEndPoint LocalEP => (IPEndPoint)_socket.LocalEndPoint;

        public DtlsListener(IPEndPoint localEP, string fileName)
        {
            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(localEP);

        }
        public DtlsClient Accept()
        {
            while (true)
            {
                if (_acceptQueue.TryDequeue(out var udpTransport))
                {
                    var random = new SecureRandom();
                    var protocol = new DtlsServerProtocol(random);
                    var server = new TlsServerImpl(_certifcate, ProtocolVersion.DTLSv12);
                    var dtlsTransport = protocol.Accept(server, udpTransport);
                    var client = new DtlsClient(dtlsTransport);
                    return client;
                }
            }
        }

        public Task<DtlsClient> AcceptAsync()
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
            return Accept();
        }

        async Task<ISslClient> ISslListener.AcceptAsync()
        {
            return await AcceptAsync();
        }
    }
}
