using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recommend.API.Data;

namespace helloApi
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //注册Consul信息（在applicationLifetime.ApplicationStarted 注册进Consult服务器）
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;//在Consul中的服务名称

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);//Consul地址
                }
            }));
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启动
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, serviceOptions, consul);
            });
            //停止
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceOptions, consul);
            });

            app.UseMvc();
        }


        private void RegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {
            //#region 注册进Consul
            //var features = app.Properties["server.Features"] as FeatureCollection;
            //var addresses = features.Get<IServerAddressesFeature>()
            //    .Addresses
            //    .Select(p => new Uri(p));
            //_logger.LogDebug($"addresses.Count{addresses.Count()}");

            //foreach (var address in addresses)
            //{
            //    var serviceId = $"{serviceOptions.Value.ContactServiceName}_{address.Host}:{address.Port}";
            //    _logger.LogDebug($"serviceId {serviceId }");
            //    var httpCheck = new AgentServiceCheck()
            //    {
            //        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
            //        Interval = TimeSpan.FromSeconds(30),
            //        HTTP = new Uri(address, "HealthCheck").OriginalString
            //    };
            //    var registration = new AgentServiceRegistration()
            //    {
            //        Checks = new[] { httpCheck },
            //        Address = address.Host,
            //        ID = serviceId,
            //        Name = serviceOptions.Value.ContactServiceName,
            //        Port = address.Port
            //    };
            //    consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            //}


            var serviceId = $"{serviceOptions.Value.ContactServiceName}_{serviceOptions.Value.ServiceIP}:{serviceOptions.Value.ServicePort}";
            var healthCheckUrl = $"http://{serviceOptions.Value.ServiceIP}:{serviceOptions.Value.ServicePort}/api/HealthCheck";
            
            _logger.LogDebug($"healthCheckUrl：{healthCheckUrl}");
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(30),
                HTTP = healthCheckUrl
            };
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                Address = serviceOptions.Value.ServiceIP,
                ID = serviceId,
                Name = serviceOptions.Value.ContactServiceName,
                Port = serviceOptions.Value.ServicePort
            };
            consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
        }


        private void DeRegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {

            #region 卸载Consul注册
            //var features = app.Properties["server.Features"] as FeatureCollection;
            //var addresses = features.Get<IServerAddressesFeature>()
            //    .Addresses
            //    .Select(p => new Uri(p));
            //foreach (var address in addresses)
            //{
            //    var serviceId = $"{serviceOptions.Value.ContactServiceName}_{address.Host}:{address.Port}";
            //    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            //}
            var serviceId = $"{serviceOptions.Value.ContactServiceName}_{serviceOptions.Value.ServiceIP}:{serviceOptions.Value.ServicePort}";
            consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            #endregion
        }
    }
}
