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
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Sold> Sold { get; set; }
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

                entity.Property(e => e.PreviousOrder).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Customer");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "Store");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Orders__C3905BCFDA62ABDE");

                entity.ToTable("Orders", "Store");

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerId_Customer");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_LocationId_Location");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Store");

                entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Sold>(entity =>
            {
                entity.HasKey(e => e.Sold1)
                    .HasName("PK__Sold__BC3BCFCAAA7D8A9C");

                entity.ToTable("Sold", "Store");

                entity.Property(e => e.Sold1).HasColumnName("Sold");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Sold)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderID_Orders");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock", "Store");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_LocationId2_Location");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductId2_Product");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
