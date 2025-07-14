using FinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Data
{
    public static class SeedData
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                });
            }

            
            var user = await userManager.FindByEmailAsync("kaan@info");
            if (user == null)
            {
                var newUser = new User
                {
                    UserName = "kaan@info",
                    Email = "kaan@info",
                    FullName="kaankaya",                   
                    EmailConfirmed = false,
                };

                var result = await userManager.CreateAsync(newUser, "123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Admin");
                    await userManager.UpdateAsync(newUser);
                }
                else
                {
                   
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
        }
    }
}
