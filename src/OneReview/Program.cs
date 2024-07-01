using OneReview.Persistence.Database;
using OneReview.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddScoped<ProductsService>();
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    app.MapControllers();

    DbInitializer.Initialize(app.Configuration["Database:ConnectionStrings:DefaultConnection"]!);
}

app.Run();