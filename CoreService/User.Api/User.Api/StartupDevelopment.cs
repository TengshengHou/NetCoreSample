using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.Api.Data;
using User.Api.Filters;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace User.Api
{
    public class StartupDevelopment
    {
        public StartupDevelopment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(options =>
            {
                //options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
                 options.UseNpgsql(Configuration.GetConnectionString("PGConnection"));
                //options.UseSqlServer(Configuration.GetConnectionString("sqlservice"), sqlOptions => sqlOptions.UseRowNumberForPaging());
            });

            //认证注册
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options =>
            {
                Options.RequireHttpsMetadata = false;
                Options.Audience = "user_api";
                Options.Authority = "http://localhost:81";//网关地址
            });

            //注册配置文件
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

            services.AddMvc(options =>
            {
                //自定义全局异常过滤器
                options.Filters.Add<HttpGlobalExceptionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //CAP
            services.AddCap(options =>
            {
                options.UseEntityFramework<UserContext>().UseRabbitMQ("192.168.2.2").UseDashboard();
                // Register to Consul
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5000;
                    d.NodeId = 2;
                    d.NodeName = "CAP User API";
                });
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul, ILoggerFactory loggerFactory)
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

            RegisterZipkinTrace(app, loggerFactory, applicationLifetime);


            app.UseCap();
            app.UseAuthentication();
            app.UseMvc();
        }


        private void RegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {
            #region 注册进Consul
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };
                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };
                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
            #endregion
        }

        private void DeRegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {

            #region 卸载Consul注册
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
            #endregion
        }

        private void RegisterZipkinTrace(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender("http://192.168.2.2:9411", "application/json");
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());

                var consoleTracer = new zipkin4net.Tracers.ConsoleTracer();
                TraceManager.RegisterTracer(consoleTracer);

                TraceManager.RegisterTracer(tracer);
                TraceManager.Start(logger);
            });
            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());
            app.UseTracing("User.Api");
        }

    }
}


