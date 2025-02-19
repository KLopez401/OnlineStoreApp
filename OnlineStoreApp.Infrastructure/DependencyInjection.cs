using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Infrastructure.Context;
using OnlineStoreApp.Infrastructure.Repositories;

namespace OnlineStoreApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerPurchaseRepository, CustomerPurchaseRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
