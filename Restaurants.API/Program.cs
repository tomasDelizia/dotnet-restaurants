using Restaurants.API.Extensions;
using Restaurants.API.Middleware;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;
// Register the presentation services
builder.AddPresentation();
// Register the application services
builder.Services.AddApplicationServices(config);
// Register the infrastructure services
builder.Services.AddInfrastructure(config);

var app = builder.Build();

// Seed DB
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options.AddPreferredSecuritySchemes("Bearer"));
}

// Define Middleware pipiline to use in order

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseMiddleware<TimeLoggingMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapGroup("api/identity").MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
