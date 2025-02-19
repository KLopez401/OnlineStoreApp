using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OnlineStoreApp.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var typesWithInterfaces = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Implementation = t,
                Interface = t.GetInterfaces().FirstOrDefault()
            })
            .Where(t => t.Interface != null); 

        foreach (var type in typesWithInterfaces)
        {
            services.AddScoped(type.Interface, type.Implementation);
        }

        return services;
    }
}