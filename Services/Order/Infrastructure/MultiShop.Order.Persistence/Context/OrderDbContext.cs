using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MultiShop.Order.Persistence.Context
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Ordering> Orderings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(x => x.ProductPrice).HasPrecision(18, 2);
                entity.Property(x => x.ProductTotalPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Ordering>(entity =>
            {
                entity.Property(x => x.TotalPrice).HasPrecision(18, 2);
            });
        }

    }
}
