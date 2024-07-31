using Microsoft.AspNetCore.Diagnostics;
using OneReview.Errors;
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

            if (exception is null)
            {
                return Results.Problem();
            }

            return exception switch
            {
                ServiceException serviceException => Results.Problem(
                    statusCode: serviceException.StatusCode,
                    detail: serviceException.ErrorMessage),
                _ => Results.Problem()
            };
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