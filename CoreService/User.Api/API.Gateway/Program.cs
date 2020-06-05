using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gateway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.FullName;
            return WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((webHost, builder) =>
            {
                builder.SetBasePath(webHost.HostingEnvironment.ContentRootPath)
                .AddJsonFile("Ocelot.json");
            })
           .UseStartup(assemblyName)
            .UseUrls("http://*:81");
        }
    }
}
