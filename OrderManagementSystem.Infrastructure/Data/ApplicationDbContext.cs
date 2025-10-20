// Infrastructure/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer Configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // CustomerAddress Configuration
            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId);
                entity.Property(e => e.AddressType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Town).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.Customer)
                      .WithMany(e => e.CustomerAddresses)
                      .HasForeignKey(e => e.CustomerId);
            });

            // Stock Configuration
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.StockId);
                entity.Property(e => e.StockName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Unit).HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Barcode).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Barcode).IsUnique();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.OrderNo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Tax).HasColumnType("decimal(18,2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.Customer)
                      .WithMany(e => e.Orders)
                      .HasForeignKey(e => e.CustomerId);

                entity.HasOne(e => e.DeliveryAddress)
                      .WithMany(e => e.DeliveryOrders)
                      .HasForeignKey(e => e.DeliveryAddressId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.InvoiceAddress)
                      .WithMany(e => e.InvoiceOrders)
                      .HasForeignKey(e => e.InvoiceAddressId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderDetail Configuration
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderDetails)
                      .HasForeignKey(e => e.OrderId);

                entity.HasOne(e => e.Stock)
                      .WithMany(e => e.OrderDetails)
                      .HasForeignKey(e => e.StockId);
            });
        }
    }
}