using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading;

namespace PwdClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000 * 10);
            var diso = DiscoveryClient.GetAsync("http://localhost:5000").Result;

            var tokenClient = new TokenClient(diso.TokenEndpoint, "pwdclient", "secret");
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("admin","123456").Result;
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }
            else
            {
                Console.WriteLine(tokenResponse.AccessToken);
            }
            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response = httpClient.GetAsync("http://localhost:5001/api/values").Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            Console.ReadLine();
        }
    }
}
