using DnsClient;
using System;
using System.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var dnsQuery = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
            var result = dnsQuery.ResolveService("service.consul", "SERVICENAME");
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
