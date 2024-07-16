using OneReview.DependencyInjection;
using OneReview.RequestPipeline;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddServices()
        .AddPersistence(builder.Configuration)
        .AddControllers();
}

var app = builder.Build();
{
    app.MapControllers();
    app.InitializeDatabase();
}

app.Run();