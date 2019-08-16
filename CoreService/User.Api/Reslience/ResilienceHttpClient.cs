using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;

namespace Reslience
{
    public class ResilienceHttpClient : IHttpClient
    {
        private HttpClient _httpClient;
        //根据url origin 去创建policy
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;
        //把去创建policy 打包组合 policy wraper 进行本地缓存
        private readonly ConcurrentDictionary<string, PolicyWrap> _policyWraps;
        private ILogger _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public ResilienceHttpClient(Func<string, IEnumerable<Policy>> policyCreator, ILogger<ResilienceHttpClient> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _policyWraps = new ConcurrentDictionary<string, PolicyWrap>();
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<HttpResponseMessage> PostAsync<T>(string url, T item,
            string authorizationToken, string requestId = null, string authorizationMethod = "Bearer")
        {
            return null;
        }

        public  
    }
}
