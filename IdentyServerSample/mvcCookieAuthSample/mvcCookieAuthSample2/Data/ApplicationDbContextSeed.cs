using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using mvcCookieAuthSample.Models;
using Microsoft.Extensions.DependencyInjection;

namespace mvcCookieAuthSample.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;

        private RoleManager<ApplicationUserRole> _roleManager;
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (!context.Users.Any())
            {
                _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                _roleManager = services.GetRequiredService<RoleManager<ApplicationUserRole>>();

                if (!context.Roles.Any()) {
                    var role = new ApplicationUserRole() { Name = "Administrators", NormalizedName = "Administrators" };
                    var ret = await _roleManager.CreateAsync(role);
                    if (!ret.Succeeded) {
                        throw new Exception("初始化角色失败"+ret.Errors.SelectMany(e=>e.Description));
                    }
                }

                var defaultUser = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "jessetalk@163.com",
                    NormalizedUserName = "admin",
                    SecurityStamp="admin",
                    Avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTY_6tPDEd_cbogLHLoQXdEg1P3jKR_Ad56lmWv3TItiHtNAjLvLA"
                };

                
                var result = await _userManager.CreateAsync(defaultUser, "123456");
                await _userManager.AddToRoleAsync(defaultUser, "Administrators");
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败");
                }
            }
        }
    }
}
