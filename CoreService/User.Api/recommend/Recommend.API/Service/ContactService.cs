using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommend.API.Data;
using Recommend.API.Dtos;
using Reslience;

namespace Recommend.API.Service
{
    public class ContactService : IContactService
    {
        private IHttpClient _httpClient;
        private string _userContactUrl;
        private ILogger<ContactService> _logger;
        public ContactService(IHttpClient httpClient, IDnsQuery dnsquery, IOptions<ServiceDisvoveryOptions> serviceDisvoveryOptions, ILogger<ContactService> logger)
        {
            _httpClient = httpClient;
            var address = dnsquery.ResolveService("service.consul", serviceDisvoveryOptions.Value.ContactServiceName);
            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : addressList.First().Address.ToString();
            var port = address.First().Port;
            _userContactUrl = $"http://{host}:{port}";
            _logger = logger;
        }
        public async Task<List<Contact>> GetContactsByUserId(int userId)
        {
            _logger.LogTrace($"Enter into GetCont:{ userId}");
            var url = _userContactUrl + "/api/contacts";
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                if (!string.IsNullOrEmpty(response))
                {
                    var userIdentity = JsonConvert.DeserializeObject<UserIdentity>(response);
                    _logger.LogTrace($"complete GetContactsByUserId with userid:{ userId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("complete GetContactsByUserId 在重试之后失败", ex.Message + ex.StackTrace);
                throw ex;
            }

            return null;
        }

    
    }
}
