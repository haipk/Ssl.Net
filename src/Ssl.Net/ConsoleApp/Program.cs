using System;
using Ssl.Net;
using System.Net;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DtlsListener listner = new DtlsListener(new IPEndPoint(IPAddress.Any, 5000), "server.crt", "server.key");
        }
    }
}