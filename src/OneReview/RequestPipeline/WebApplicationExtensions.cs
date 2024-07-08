using OneReview.Persistence.Database;

namespace OneReview.RequestPipeline;

public static class WebApplicationExtensions
{
    public static WebApplication InitializeDatabase(this WebApplication app)
    {
        var connectionString = app.Configuration[DbConstants.DefaultConnectionStringPath];
        DbInitializer.Initialize(connectionString!);

        return app;
    }
}