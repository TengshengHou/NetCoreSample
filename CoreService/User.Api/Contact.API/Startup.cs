using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //Service.AddCap()  97 4分时有看到此代码
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())

                app.UseDeveloperExceptionPage();
            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}

