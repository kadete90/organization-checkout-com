using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApp.Common;
using BasketApp.Api.Data;
using BasketApp.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BasketApp.Api
{
    public static class SeedData
    {
        const string TesterRole = "Administrator";

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await EnsureRolesAsync(roleManager);

                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                await EnsureTestAdminAsync(userManager);

                var context = services.GetRequiredService<ApplicationDbContext>();
                await SeedProductsAsync(context);

                logger.LogInformation("Seeded the database.");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error seeding database");
            }
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
            var tester = await userManager.Users.Where(x => x.UserName == TestConstants.TesterUserName).SingleOrDefaultAsync();
            if (tester != null)
            {
                return;
            }

            tester = new IdentityUser
            {
                UserName = TestConstants.TesterUserName,
                Email = TestConstants.TesterEmail,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult user = await userManager.CreateAsync(tester, TestConstants.TesterPassword);

            if (!user.Succeeded)
            {
                throw new Exception(string.Join("\n" ,user.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(tester, TesterRole);
        }

        private static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            List<Product> products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Milk", Price = 4.00 },
                new Product { Id = Guid.NewGuid(), Name = "Orange", Price = 2.00 },
                new Product { Id = Guid.NewGuid(), Name = "Chocolate", Price = 3.00 },
                new Product { Id = Guid.NewGuid(), Name = "Cookies", Price = 5.50 },
                new Product { Id = Guid.NewGuid(), Name = "Bread", Price = 1.00 }
            };

            foreach (var product in products)
            {
                if (!await context.Products.AnyAsync(p => p.Name == product.Name))
                {
                    await context.Products.AddAsync(product);
                }
            }

            await context.SaveChangesAsync();
        }

    }
}