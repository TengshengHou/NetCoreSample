using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Reslience;
using System;
using System.Net.Http;

namespace Recommend.API.infrastructure
{
    public class ResilienceClientFactory
    {
        private ILogger _logger;
        private IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// 重试次数
        /// </summary>
        private int _retryCount;
        /// <summary>
        /// 熔断之前允许的异常次数
        /// </summary>
        private int _execptionCountAllowedBeforeBreaking;

        public ResilienceClientFactory(ILogger<ResilienceClientFactory> logger, IHttpContextAccessor httpContextAccessor, int retryCount, int execptionCountAllowedBeforeBreaking)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _execptionCountAllowedBeforeBreaking = execptionCountAllowedBeforeBreaking;

        }

        public ResilienceHttpClient GetResilienceHttpClient() => new ResilienceHttpClient( origin => CreatePolicys(origin), _logger, _httpContextAccessor);


        private Policy[] CreatePolicys(string origin)
        {
            return new Policy[] {
                Policy.Handle<HttpRequestException>().WaitAndRetryAsync(_retryCount,retryAttemp=>TimeSpan.FromSeconds(Math.Pow(2,retryAttemp)),
                (exception,stimeSpan,retryCount,context)=>{
                    var msg=$"第{retryCount}次重试"+
                    $"of {context.PolicyKey}"+
                    $"at {context.ExecutionKey}"+
                    $"dut to : {exception} .";
                    _logger.LogWarning(msg);
                    _logger.LogDebug(msg);
                }),
                Policy.Handle<HttpRequestException>().CircuitBreakerAsync(_execptionCountAllowedBeforeBreaking, TimeSpan.FromMinutes(1), (excpetion, dutation) =>
                {
                    _logger.LogWarning("熔断器打开");
                }, () =>
                {
                    _logger.LogWarning("关闭");
                })
            };

        }
    }
}
