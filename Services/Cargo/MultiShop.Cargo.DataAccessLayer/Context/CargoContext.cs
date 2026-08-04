using Microsoft.EntityFrameworkCore;
using MultiShop.Cargo.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DataAccessLayer.Context
{
    public class CargoContext : DbContext
    {
        public CargoContext(DbContextOptions<CargoContext> options) : base(options)
        {
        }

        public DbSet<CargoCompany> CargoCompanies { get; set; } = null!;
        public DbSet<CargoCustomer> CargoCustomers { get; set; } = null!;
        public DbSet<CargoDetail> CargoDetails { get; set; } = null!;
        public DbSet<CargoOperation> CargoOperations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CargoDetail>()
                .HasIndex(x => x.Barcode)
                .IsUnique();

            modelBuilder.Entity<CargoOperation>()
                .HasIndex(x => x.Barcode);

            modelBuilder.Entity<CargoDetail>()
                .HasOne(x => x.CargoCompany)
                .WithMany()
                .HasForeignKey(x => x.CargoCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
