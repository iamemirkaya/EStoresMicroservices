using Basket.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Basket.API.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>()
                .HasKey(s => s.UserName);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(i => i.ShoppingCartUserName)
                .OnDelete(DeleteBehavior.Cascade); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
