using Restaurants.API.Middleware;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register the application services
builder.Services.AddApplicationServices(config);
// Register the infrastructure services
builder.Services.AddInfrastructure(config);
// Add logging
builder.Host.UseSerilog((context, cfg) =>
    // Retrieve from appsettings.Development.json
    cfg.ReadFrom.Configuration(config)
    // cfg
    //     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    //     .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    //     .WriteTo.File("Logs/Restaurants-API-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    //     .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] | {SourceContext} | {Message:lj}{NewLine}{Exception}")
);
// Add error handling middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<TimeLoggingMiddleware>();

var app = builder.Build();

// Seed DB
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Define Middleware pipiline to use in order

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseMiddleware<TimeLoggingMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
