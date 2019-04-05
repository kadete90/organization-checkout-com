using System;
using BasketApp.Api.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasketApp.Api.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProducts> BasketProducts { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BasketProducts>().HasKey(x => new { x.BasketId, x.ProductId });

            //builder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Milk", Price = 4.00 });
            //builder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Orange", Price = 2.00 });
            //builder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Chocolate", Price = 3.00 });
            //builder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Cookies", Price = 5.50 });
            //builder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Bread", Price = 1.00 });
        }
    }
}
