using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rena.Domain.Entities;
using Rena.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rena.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Size)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Condition)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(ProductStatus.Available);

            builder.Property(p => p.ForSale)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.ForRent)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt);

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.CategoryId);
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.Price);
            builder.HasIndex(p => p.CreatedAt);
            builder.HasIndex(p => p.IsActive);

            builder.HasIndex(p => new { p.Title, p.Description })
                .HasDatabaseName("IX_Products_Search");

            // Relationships
            builder.HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
