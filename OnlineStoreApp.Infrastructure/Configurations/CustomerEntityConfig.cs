using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Domain.Entities;

namespace OnlineStoreApp.Infrastructure.Configurations;

public class CustomerEntityConfig : IEntityTypeConfiguration<Customers>
{
    public void Configure(EntityTypeBuilder<Customers> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()          
            .HasMaxLength(100);

        builder.Property(u => u.Phone)
            .HasMaxLength(100);

        builder.Property(u => u.DateAdded)
            .HasDefaultValue<DateTime>(DateTime.MinValue);

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue<bool>(false);

        builder.HasMany(c => c.CustomerPurchases)   
            .WithOne(cp => cp.Customer)       
            .HasForeignKey(cp => cp.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Customers");

    }
}
