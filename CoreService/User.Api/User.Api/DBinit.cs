using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using User.Api.Data;
#pragma warning disable CS1591
namespace Core.SimpleTemp.Mvc
{
    public class DBInitializer
    {
        private static void Initialize(UserContext context)
        {
            context.Database.EnsureCreated();
        }


        public static void Initialize(IWebHost host)
        {

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<UserContext>();
                    DBInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DBInitializer>>();
                    logger.LogError(ex, "初始化数据库异常.");
                }
            }
        }
    }
}
