using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStoreApp.Domain.Entities;

namespace OnlineStoreApp.Infrastructure.Configurations;

public class ProductEntityConfig : IEntityTypeConfiguration<Products>
{
    public void Configure(EntityTypeBuilder<Products> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Description)
            .HasMaxLength(255);

        builder.Property(u => u.Price)
            .IsRequired()
            .HasDefaultValue(0.00);

        builder.Property(u => u.DateAdded)
            .HasDefaultValue(DateTime.MinValue);

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder.ToTable("Products");
    }
}

