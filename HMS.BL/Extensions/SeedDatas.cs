using HMS.BL.Enums;
using HMS.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.BL.Extensions
{
    public static class SeedDatas
    {
        public static async void UseUserSeed(this IApplicationBuilder app)
        {
            using(var scope=app.ApplicationServices.CreateScope()) 
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(!roleManager.Roles.Any()) 
                {
                    foreach(var role in Enum.GetValues(typeof(Roles)))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                    }
                }

                if(!userManager.Users.Any(x=>x.NormalizedUserName=="ADMIN")) 
                {
                    User user=new User()
                    {
                        UserName="admin",
                        Email="admin@gmail.com"
                    };

                    await userManager.CreateAsync(user, "123");
                    await userManager.AddToRoleAsync(user,nameof(Roles.Admin));
                }

            }
        }
    }
}
