using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Separate.Data.Entities;
using Separate.Data.Enums;

namespace Separate.Data
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();
            context.Database.EnsureCreated();


            if (!context.Users.Any())
            {
                var user = new User
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = "Admin"
                };

                userManager.CreateAsync(user, "Admin1!!").Wait();

                if (!context.Roles.Any())
                {
                    roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Admin }).Wait();
                    roleManager.CreateAsync(new IdentityRole { Name = AppRoles.Client }).Wait();
                }

                userManager.AddToRoleAsync(user, AppRoles.Admin).Wait();
            }

        }
    }
}
