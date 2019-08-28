using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reslience;
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
        private IHttpClient _httpClient;
        private string _userServiceUrl;
        private ILogger<UserService> _logger;

        public UserService(IHttpClient httpClient, IDnsQuery dnsquery, IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            var address = dnsquery.ResolveService("service.consul", serviceDisvoveryOptions.Value.UserServiceName);
            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : addressList.First().Address.ToString();
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
            _logger = logger;
        }

        public async Task<int> CheckOrCreateAsync(string phone)
        {
            var form = new Dictionary<string, string>() { { "phone", phone } };
            var context = new FormUrlEncodedContent(form);
            var url = _userServiceUrl + "/api/Users/check-orcreate";
            try
            {
                var response = await _httpClient.PostAsync(url, context);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var userId = await response.Content.ReadAsStringAsync();
                    int.TryParse(userId, out int intUserId);
                    _logger.LogTrace($"complete CheckOrCreateAsync with userid:{ userId}");
                    return intUserId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("complete CheckOrCreateAsync 在重试之后失败", ex.Message + ex.StackTrace);
                throw ex;
            }

            return 0;
        }


     

    }
}
