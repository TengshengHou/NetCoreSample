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
            //检查是否需要初始化数据
            if (!context.Users.Any())
            {
                context.Users.Add(new User.Api.Model.AppUser()
                {
                    Name = "admin",
                });
            }
            if (!context.UserProperty.Any())
            {
                var adminId = context.Users.SingleOrDefault(u => u.Name == "admin").Id;
                context.UserProperty.Add(new User.Api.Model.UserProperty()
                {
                    AppUserId = adminId,
                    Key = "key1",
                    Value = "Value1"
                });
                context.UserProperty.Add(new User.Api.Model.UserProperty()
                {
                    AppUserId = adminId,
                    Key = "key2",
                    Value = "Value2"
                });
            }

            context.SaveChanges();
        }


        public static void Initialize(IWebHost host)
        {

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<UserContext>();
                    Initialize(context);
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
