using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarStore.DataAccess.Model
{
    public partial class Project0Context : DbContext
    {
        public Project0Context()
        {
        }

        public Project0Context(DbContextOptions<Project0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<OrderLine> OrderLine { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Store");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(26);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(26);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "Store");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK_OrderId_ProductId");

                entity.ToTable("OrderLine", "Store");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderLine_OrderId_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderLine_ProductId_Product");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Orders__C3905BCF7898E3E1");

                entity.ToTable("Orders", "Store");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_CustomerId_Customer");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Orders_LocationId_Location");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Store");

                entity.HasIndex(e => e.ProductName)
                    .HasName("UQ__Product__DD5A978AC2B0EB9A")
                    .IsUnique();

                entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock", "Store");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_LocationId_Location");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductId_Product");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
