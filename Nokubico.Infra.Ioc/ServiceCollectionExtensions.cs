using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connection))
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connection, b => b.MigrationsAssembly("Nokubico.Infra.Data"))
                );
            }

            return services;
        }
    }
}
