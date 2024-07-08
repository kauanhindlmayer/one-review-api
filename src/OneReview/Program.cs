using OneReview.DependencyInjection;
using OneReview.RequestPipeline;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddServices()
        .AddControllers();
}

var app = builder.Build();
{
    app.MapControllers();
    app.InitializeDatabase();
}

app.Run();