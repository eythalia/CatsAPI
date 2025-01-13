using CatImageApi.Application.Interfaces;
using CatImageApi.Application.Services;
using CatImageApi.Infrastructure.Persistence;
using CatImageApi.Infrastructure.Repositories;
using CatImageApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CatImageApi.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("CatDbConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,                // Maximum number of retries
                        maxRetryDelay: TimeSpan.FromSeconds(10), // Maximum delay between retries
                        errorNumbersToAdd: null)));     // Add SQL error numbers to handle (optional)

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ICatProviderService, CatProviderService>();

            return services;
        }

        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICatService, CatService>();
            services.AddScoped<IDbRepository, DbRepository>();
            services.AddScoped<ICatProviderService, CatProviderService>();

            return services;
        }
    }
}
