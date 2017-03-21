using System.Net;
using System.Threading.Tasks;

namespace Ssl.Net
{
    public interface ISslListener
    {
        IPEndPoint LocalEP { get; }
        ISslClient Accept();
        Task<ISslClient> AcceptAsync();
        void Start();
        void Stop();
    }
}