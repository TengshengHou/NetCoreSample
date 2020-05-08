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
        //public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        //  .SetBasePath(Directory.GetCurrentDirectory())
        //  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //  .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        //  .AddEnvironmentVariables()
        //  .Build();
        public static void Main(string[] args)
        {
            // Log.Logger = new LoggerConfiguration()
            //.ReadFrom.Configuration(Configuration)
            ////.MinimumLevel.Debug()
            ////.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
            //.Enrich.FromLogContext()
            ////.WriteTo.Console()
            //.CreateLogger();

            try
            {
                //Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                ///Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                //Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>().UseUrls("http://47.100.193.29:5021");
        //.UseStartup<Startup>().UseSerilog(
        //(ctx, config) =>
        //{
        //    config.ReadFrom.Configuration(ctx.Configuration);
        //    config.WriteTo.Console(new ElasticsearchJsonFormatter());
        //}
        //); // <-- Add this line;;
    }
}
