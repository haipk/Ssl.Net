using System.Net;
using System.Threading.Tasks;

namespace Ssl.Net
{
    public interface ISslClient
    {
        bool Connected { get; }
        IPEndPoint LocalEp { get; }
        IPEndPoint RemoteEP { get; }

        void Close();
        void Connect(IPEndPoint remoteEP);
        Task ConnectAsync(IPEndPoint remoteEP);
        int Receive(byte[] buffer, int offset, int count,int waitMillis);
        Task<int> ReceiveAsync(byte[] buffer, int offset, int count);
        int Send(byte[] buffer, int offset, int count);
        Task<int> SendAsync(byte[] buffer, int offset, int count);
    }
}