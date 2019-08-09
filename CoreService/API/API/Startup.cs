using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            applicationLifetime.ApplicationStarted.Register(OnStart);
            applicationLifetime.ApplicationStopped.Register(OnStopped);
            app.UseMvc();
        }


        private void OnStopped() {
            var client = new ConsulClient();
            client.Agent.ServiceDeregister("SERVICENAME:5001");
        }
        private void OnStart()
        {
            var client = new ConsulClient();
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter=TimeSpan.FromMinutes(1),
                Interval=TimeSpan.FromSeconds(30),
                HTTP= "http://127.0.0.1:5001/api/HealthCheck"
            };
            var agentReg = new AgentServiceRegistration()
            {
                ID = "TESTAPI",
                Check = httpCheck,
                Address = "127.0.0.1",
                Name = "SERVICENAME",
                Port = 5001
            };
            client.Agent.ServiceRegister(agentReg).ConfigureAwait(false);


        }
    }
}
