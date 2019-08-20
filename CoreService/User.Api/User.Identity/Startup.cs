using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reslience;
using User.Identity.Authentication;
using User.Identity.Dto;
using User.Identity.infrastructure;
using User.Identity.Services;

namespace User.Identity
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

            services.AddIdentityServer()
                .AddExtensionGrantValidator<SmsAuthCodeValidator>()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(Config.GetClient())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetResources());
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IDnsQuery>(p =>
             {
                 var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                 return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
             });
            //services.AddSingleton(new HttpClient());
            services.AddSingleton(typeof(ResilienceClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilienceClientFactory>>();

                return new ResilienceClientFactory();
            });
            services.AddSingleton<IHttpClient>(sp =>
            {
                return sp.GetRequiredService<ResilienceClientFactory>().GetResilienceHttpClient();

            });
            services.AddScoped<IAuthCodeService, TestAuthCodeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}
