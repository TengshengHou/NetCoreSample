using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Elasticsearch;

namespace helloApi
{
    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
        //.UseStartup<Startup>();
        .UseStartup<Startup>().UseSerilog(
        (ctx, config) =>
        {
            config.ReadFrom.Configuration(ctx.Configuration);
            config.WriteTo.Console(new ElasticsearchJsonFormatter());
        }
        );
    }
}
