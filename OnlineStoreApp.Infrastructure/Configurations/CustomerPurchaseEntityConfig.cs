using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStoreApp.Domain.Entities;

namespace OnlineStoreApp.Infrastructure.Configurations;

public class CustomerPurchaseEntityConfig : IEntityTypeConfiguration<CustomerPurchases>
{
    public void Configure(EntityTypeBuilder<CustomerPurchases> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Products)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(u => u.Total)
            .IsRequired()
            .HasDefaultValue(0.00);

        builder.Property(u => u.PurchaseDate)
            .IsRequired();

        builder.Property(u => u.ReceiptReference)
            .HasMaxLength(100);

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder.ToTable("CustomerPurchases");
    }
}