using System;
using Ssl.Net;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = Utilities.LoadPrivateKeyResource(@"E:\Develop\Company\GuarderServer\GuarderServer\server.key");
        }
    }
}