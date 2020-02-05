using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recommend.API.Data;
using Recommend.API.infrastructure;
using Recommend.API.Service;
using Reslience;

namespace Recommend.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //注册配置文件
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //注册业务服务
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContactService, ContactService>();

            //提供Consul调用支持
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });

            //注册全局单例ResilienceClientFactory
            services.AddSingleton(typeof(ResilienceClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilienceClientFactory>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 5;
                var execptionCountAllowedBeforeBreaking = 5;
                return new ResilienceClientFactory(logger, httpContextAccessor, retryCount, execptionCountAllowedBeforeBreaking);
            });
            services.AddSingleton<IHttpClient>(sp =>
            {
                return sp.GetRequiredService<ResilienceClientFactory>().GetResilienceHttpClient();
            });

            //数据库
            services.AddDbContext<RecommendDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("sqlservice"), sqlOptions =>
                {
                    //sqlOptions.UseRowNumberForPaging();
                    //sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name);
                });
            });

            //CAP
            services.AddCap(options =>
            {

                options.UseEntityFramework<RecommendDbContext>().UseRabbitMQ("localhost").UseDashboard();
                // Register to Consul
                options.UseDiscovery(d =>
                {
                    
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;//Consul配置
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5004;//当前项目启动端口
                    d.NodeId = 3;
                    d.NodeName = "CAP Recommend api";
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCap();
            app.UseMvc();
        }
    }
}
