using OneReview.Persistence.Database;
using OneReview.Persistence.Repositories;
using OneReview.Services;

namespace OneReview.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            };
        });

        return services;
    }

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
            var connectionString = configuration[DbConstants.DefaultConnectionStringPath]!;
            return new NpgsqlConnectionFactory(connectionString);
        });
        services.AddScoped<ProductsRepository>();
        services.AddScoped<ReviewsRepository>();

        return services;
    }
}