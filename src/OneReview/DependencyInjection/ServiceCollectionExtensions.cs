using OneReview.Persistence.Database;
using OneReview.Persistence.Repositories;
using OneReview.Services;

namespace OneReview.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ProductsService>();
        services.AddScoped<ReviewsService>();

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IDbConnectionFactory, NpgsqlConnectionFactory>(_ =>
        {
            return new NpgsqlConnectionFactory(configuration[DbConstants.DefaultConnectionStringPath]!);
        });
        services.AddScoped<ProductsRepository>();

        return services;
    }
}