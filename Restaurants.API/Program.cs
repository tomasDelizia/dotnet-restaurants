using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;
using Serilog.Events;

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
builder.Host.UseSerilog((context, config) =>
    config
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] | {SourceContext} | {Message:lj}{NewLine}{Exception}")
);

var app = builder.Build();

// Get more information about the http requests in the logs
app.UseSerilogRequestLogging();

// Seed DB
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
