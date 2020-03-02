using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Wrap;
using zipkin4net.Transport.Http;

namespace Reslience
{
    public class ResilienceHttpClient : IHttpClient
    {
        private HttpClient _httpClient;
        //根据url origin 去创建policy
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;
        //把去创建policy 打包组合 policy wraper 进行本地缓存
        private readonly ConcurrentDictionary<string, PolicyWrap> _policyWrappers;
        private ILogger _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public ResilienceHttpClient(string applicationName, Func<string, IEnumerable<Policy>> policyCreator, ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient(new TracingHandler(applicationName));
            _policyWrappers = new ConcurrentDictionary<string, PolicyWrap>();
            _logger = logger;
            _policyCreator = policyCreator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            //Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Post, url, item);
            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Put, url, item);
            return await DoPostPutAsync(HttpMethod.Put, url, func, authorizationToken, requestId, authorizationMethod);
        }

        public async Task<string> GetStringAsync(string url, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUrl(url);
            return await HttpInvoker(origin, async () =>
            {

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                SetAuthorizationHeader(requestMessage);
                if (authorizationToken != null)
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new HttpRequestException();
                if (!response.IsSuccessStatusCode)
                    return null;
                return await response.Content.ReadAsStringAsync();
            });
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage<T>(HttpMethod.Post, url, item);
            return await DoPostPutAsync(HttpMethod.Post, url, func, authorizationToken, requestId, authorizationMethod);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> form,
string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {

            Func<HttpRequestMessage> func = () => CreateHttpRequestMessage(HttpMethod.Post, url, form);
            return await DoPostPutAsync(HttpMethod.Post, url, func, authorizationToken, requestId, authorizationMethod);
        }


        private async Task<HttpResponseMessage> DoPostPutAsync(HttpMethod method, string url, Func<HttpRequestMessage> requestMessageAction, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("", nameof(method));
            }
            var origin = GetOriginFromUrl(url);
            return await HttpInvoker(origin, async () =>
            {

                HttpRequestMessage requestMessage = requestMessageAction();
                SetAuthorizationHeader(requestMessage);
                if (authorizationToken != null)
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                if (requestId != null)
                    requestMessage.Headers.Add("x-requestid", requestId);
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new HttpRequestException();
                return response;
            });

        }

        private async Task<T> HttpInvoker<T>(string origin, Func<Task<T>> action)
        {
            var normalizedOrigin = NormalizedOrigin(origin);
            if (!_policyWrappers.TryGetValue(normalizedOrigin, out PolicyWrap policyWrap))
            {
                var v = _policyCreator(normalizedOrigin).ToArray();
                policyWrap = Policy.WrapAsync(v);
                _policyWrappers.TryAdd(normalizedOrigin, policyWrap);
            }
            return await policyWrap.ExecuteAsync(action,
                                                 new Context(normalizedOrigin));
        }

        private static string NormalizedOrigin(string origin)
        {
            return origin.Trim()?.ToLower();
        }
        private static string GetOriginFromUrl(string uri)
        {
            var url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;

        }
        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string> { authorizationHeader });
            }

        }

        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method, string url, T item)
        {
            var requestMessage = new HttpRequestMessage(method, url);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            return requestMessage;
        }
        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url, Dictionary<string, string> form)
        {
            var requestMessage = new HttpRequestMessage(method, url);
            requestMessage.Content = new FormUrlEncodedContent(form);
            return requestMessage;
        }



    }
}
