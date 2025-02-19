using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Domain.Entities;
using OnlineStoreApp.Infrastructure.Configurations;

namespace OnlineStoreApp.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    public DbSet<Customers> Customers { get; set; }
    public DbSet<CustomerPurchases> CustomerPurchases { get; set; }
    public DbSet<Products> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityConfig());
        modelBuilder.ApplyConfiguration(new CustomerPurchaseEntityConfig());
        modelBuilder.ApplyConfiguration(new ProductEntityConfig());

        base.OnModelCreating(modelBuilder);
    }
}