using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using User.Api.Data;
using User.Api.Filters;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace User.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PGConnection"));
            });

            //认证注册
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options =>
            {
                Options.RequireHttpsMetadata = false;
                Options.Audience = "user_api";
                Options.Authority = "http://47.100.193.29:81";//网关地址
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
                options.UseEntityFramework<UserContext>().UseRabbitMQ(rabbitMQOptions =>
                {
                    rabbitMQOptions.UserName = "guest";
                    rabbitMQOptions.Password = "guest123";
                    rabbitMQOptions.Port = 5672;
                    rabbitMQOptions.HostName = "47.100.193.29";
                }).UseDashboard();
                // Register to Consul
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "47.100.193.29";
                    d.DiscoveryServerPort = 8501;
                    d.CurrentNodeHostName = "47.100.193.29";
                    d.CurrentNodePort = 82;
                    d.NodeId = 2;
                    d.NodeName = "CAP User API";
                });
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul, ILoggerFactory loggerFactory, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启动
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, serviceOptions, consul, logger);
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


        private void RegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul, ILogger<Startup> logger)
        {
            #region 注册进Consul
            var serviceId = $"{serviceOptions.Value.ServiceName}_{serviceOptions.Value.ServiceIP}:{serviceOptions.Value.ServicePort}";
            var healthCheckUrl = $"http://{serviceOptions.Value.ServiceIP}:{serviceOptions.Value.ServicePort}/HealthCheck";
            logger.LogDebug($"注册进Consul serviceId:{serviceId} ,healthCheckUrl：{healthCheckUrl}");
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
                Name = serviceOptions.Value.ServiceName,
                Port = serviceOptions.Value.ServicePort
            };
            var serviceRegisterResutl = consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            logger.LogDebug($"注册结果serviceRegisterResutl  :{serviceRegisterResutl .StatusCode}");
            #endregion
        }

        private void DeRegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {

            #region 卸载Consul注册
            var serviceId = $"{serviceOptions.Value.ServiceName}_{serviceOptions.Value.ServiceIP}:{serviceOptions.Value.ServicePort}";
            consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            #endregion
        }

        private void RegisterZipkinTrace(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender("http://47.100.193.29:9411", "application/json");
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


