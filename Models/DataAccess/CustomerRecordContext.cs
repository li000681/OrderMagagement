using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FinalProject.Models.DataAccess
{
    public partial class CustomerRecordContext : DbContext
    {
        public CustomerRecordContext()
        {
        }

        public CustomerRecordContext(DbContextOptions<CustomerRecordContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json");
                IConfiguration Configuration = builder.Build();
                string connectionString = Configuration.GetConnectionString("CustomerRecord");
                optionsBuilder.UseSqlServer(connectionString);
            }
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasMaxLength(16);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Item__A25C5AA66C7AE203");

                entity.ToTable("Item");

                entity.Property(e => e.Code).HasMaxLength(16);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrId)
                    .HasName("PK__Order__E16497A8C5102603");

                entity.ToTable("Order");

                entity.Property(e => e.OrId).HasMaxLength(16);

                entity.Property(e => e.CustomerId).HasMaxLength(16);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_ToCustomer");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ItemCode })
                    .HasName("PK__OrderIte__607C9B5103B996D9");

                entity.ToTable("OrderItem");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(16)
                    .HasColumnName("OrderID");

                entity.Property(e => e.ItemCode).HasMaxLength(16);

                entity.HasOne(d => d.ItemCodeNavigation)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ItemCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Item");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Order");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
