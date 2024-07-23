using Microsoft.AspNetCore.Diagnostics;
using OneReview.Persistence.Database;

namespace OneReview.RequestPipeline;

public static class WebApplicationExtensions
{
    public static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler("/error");

        app.Map("/error", (HttpContext httpContext) =>
        {
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: exception?.Message);
        });

        return app;
    }

    public static WebApplication InitializeDatabase(this WebApplication app)
    {
        var connectionString = app.Configuration[DbConstants.DefaultConnectionStringPath];
        DbInitializer.Initialize(connectionString!);

        return app;
    }
}