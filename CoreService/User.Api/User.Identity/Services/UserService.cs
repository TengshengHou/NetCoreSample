using DnsClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using User.Identity.Dto;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        private string _userServiceUrl;


        public UserService(HttpClient httpClient, IDnsQuery dnsquery, IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions)
        {
            _httpClient = httpClient;
            var address = dnsquery.ResolveService("service.consul", serviceDisvoveryOptions.Value.UserServiceName);
            var addressList = address.First().AddressList;
            var host = addressList.Any()? addressList.First().ToString() : addressList.First().Address.ToString();
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
        }

        public async Task<int> CheckOrCreateAsync(string phone)
        {
            var form = new Dictionary<string, string>() { { "phone", phone } };
            var context = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync(_userServiceUrl + "/api/Users/check-orcreate", context);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var userId = await response.Content.ReadAsStringAsync();
                int.TryParse(userId, out int intUserId);
                return intUserId;
            }
            return 0;
        }
    }
}
