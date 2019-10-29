using Contact.Api.Data;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reslience;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Service
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

        public async Task<UserIdentity> GetBaseUserInfoAsync(int UserId)
        {
            var url = _userServiceUrl + "/api/Users/baseinfo/"+ UserId;
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                if (string.IsNullOrEmpty(response))
                {
                    var userIdentity = JsonConvert.DeserializeObject<UserIdentity>(response);
                    _logger.LogTrace($"complete CheckOrCreateAsync with userid:{ userIdentity.UserId}");
                    return userIdentity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("complete CheckOrCreateAsync 在重试之后失败", ex.Message + ex.StackTrace);
                throw ex;
            }

            return null;
        }
    }
}
