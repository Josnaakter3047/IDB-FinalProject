using DataModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Threading.Tasks;
using Utilities;

namespace DataAccess
{
    public class SeedDatabase
    {
        public static async Task InitializeDbAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            EnsereDbMigrationAndUpdate(context);

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await EnsureTestAdminAsync(userManager, context);
        }

        private static void EnsereDbMigrationAndUpdate(ApplicationDbContext context)
        {
            context.Database.Migrate();
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            }
            if (!await roleManager.RoleExistsAsync(SD.Role_User))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_User));
            }
        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            try
            {
                var admin = await userManager.GetUsersInRoleAsync(SD.Role_Admin);
                if (admin.Count == 0)
                {
                    var newAdmin = new ApplicationUser()
                    {
                        UserName = "josnaallo94@gmail.com",
                        Email = "josnaallo94@gmail.com",
                        FullName = "Admin",
                        PhoneNumber = "01799955528",
                        TimeStamp = DateTime.UtcNow.AddHours(6),
                        CustomerId = "SHW0"
                    };
                    await userManager.CreateAsync(newAdmin, "ShoppingWorld@559#");
                    await userManager.AddToRoleAsync(newAdmin, SD.Role_Admin);

                    await context.UserToken.AddAsync(new UserTokens()
                    {
                        ApplicationUserId = newAdmin.Id,
                        Token = CryptographyService.Encrypt("ShoppingWorld@559#")
                    });
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
