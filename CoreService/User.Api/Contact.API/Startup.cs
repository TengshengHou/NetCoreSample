using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Data;
using Contact.API.Data;
using Contact.API.infrastructure;
using Contact.API.Service;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reslience;

namespace Contact.API
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

            services.Configure<AppSettings>(Configuration);
            services.AddTransient<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();
            services.AddTransient<IContactRepository, MongoContactRepository>();
            services.AddTransient<ContactContext>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options =>
            {
                Options.RequireHttpsMetadata = false;
                Options.Audience = "contact_api";
                Options.Authority = "http://localhost:81";
                Options.SaveToken = true;//保存token 会把token存在
            });
            services.AddHttpContextAccessor();
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });
            //services.AddScoped<IUserService, UserService>();
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
            services.AddScoped<IUserService, UserService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //Service.AddCap()  97 4分时有看到此代码
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())

                app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

