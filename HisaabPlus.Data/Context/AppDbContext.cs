using HisaabPlus.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<StockPurchase> StockPurchases { get; set; }
        public DbSet<StockPurchaseItem> StockPurchaseItems { get; set; }
        public DbSet<SupplierPayment> SupplierPayments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(p => p.PurchasePrice).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.SellingPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.CurrentStock).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.LowStockLimit).HasPrecision(18, 2);
            modelBuilder.Entity<Customer>().Property(p => p.TotalBalance).HasPrecision(18, 2);
            modelBuilder.Entity<Supplier>().Property(p => p.TotalBalance).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(p => p.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(p => p.PaidAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(p => p.RemainingAmount).HasPrecision(18, 2);
            modelBuilder.Entity<SaleItem>().Property(p => p.Quantity).HasPrecision(18, 2);
            modelBuilder.Entity<SaleItem>().Property(p => p.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<SaleItem>().Property(p => p.TotalPrice).HasPrecision(18, 2);
            modelBuilder.Entity<CustomerPayment>().Property(p => p.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<StockPurchase>().Property(p => p.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<StockPurchase>().Property(p => p.PaidAmount).HasPrecision(18, 2);
            modelBuilder.Entity<StockPurchase>().Property(p => p.RemainingAmount).HasPrecision(18, 2);
            modelBuilder.Entity<StockPurchaseItem>().Property(p => p.Quantity).HasPrecision(18, 2);
            modelBuilder.Entity<StockPurchaseItem>().Property(p => p.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<StockPurchaseItem>().Property(p => p.TotalPrice).HasPrecision(18, 2);
            modelBuilder.Entity<SupplierPayment>().Property(p => p.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Subscription>().Property(p => p.Amount).HasPrecision(18, 2);
        }
    }
}
