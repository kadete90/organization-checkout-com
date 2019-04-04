using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BasketApp.Api
{
    public static class SeedData
    {
        const string TesterRole = "Administrator";
        const string TesterUserName = "testUser";

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(TesterRole);
            if (alreadyExists)
            {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole(TesterRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            var tester = await userManager.Users.Where(x => x.UserName == TesterUserName).SingleOrDefaultAsync();
            if (tester != null)
            {
                return;
            }

            tester = new IdentityUser
            {
                UserName = TesterUserName,
                Email = TesterUserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult user = await userManager.CreateAsync(tester, "testPw1!");

            if (!user.Succeeded)
            {
                throw new Exception(string.Join("\n" ,user.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(tester, TesterRole);
        }
    }
}