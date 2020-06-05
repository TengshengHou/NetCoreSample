using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.SimpleTemp.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            DBInitializer.Initialize(webHost);
            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            //User.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null 得到Startup程序集名字从而找到对应环境的 Startup Class
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.FullName;
            return WebHost.CreateDefaultBuilder(args)
                  //.UseStartup<Startup>();
                  .UseStartup(assemblyName);
        }
    }
}
