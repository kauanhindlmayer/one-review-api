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
}