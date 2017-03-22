using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ssl.Net
{
    public class DtlsClient : ISslClient
    {
        private DtlsTransport _transport;
        private Socket _socket;
        public bool Connected => throw new NotImplementedException();

        public IPEndPoint LocalEp => throw new NotImplementedException();

        public IPEndPoint RemoteEP => throw new NotImplementedException();

        internal DtlsClient(Socket socket, DtlsTransport transport)
        {
            _socket = socket;
            _transport = transport;
        }

        public DtlsClient()
        {
            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, 0));
        }
        public void Close()
        {
            _transport.Close();
        }

        public void Connect(IPEndPoint remoteEP)
        {
            _socket.Connect(remoteEP);
            var udpTransport = new UdpTransport(_socket);
            var random = new SecureRandom();
            var protocol = new DtlsClientProtocol(random);
            var client = new TlsClientImpl(ProtocolVersion.DTLSv12);
            var dtlsTransport = protocol.Connect(client, udpTransport);
        }

        public Task ConnectAsync(IPEndPoint remoteEP)
        {
            throw new NotImplementedException();
        }

        public int Receive(byte[] buffer, int offset, int count)
        {
            return _transport.Receive(buffer, offset, count, 60);
        }

        public Task<int> ReceiveAsync(byte[] buffer, int offset, int count)
        {
            return _transport.ReceiveAsync(buffer, offset, count);
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
