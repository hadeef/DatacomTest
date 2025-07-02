using DatacomTest.Server.Repositories;
using DatacomTest.Server.Repositories.Interfaces;
using DatacomTest.Server.Services;
using DatacomTest.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatacomTest.Server;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add CORS policy
        _ = builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                _ = policy.WithOrigins("https://localhost:38177") // Angular dev server
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Add services to the container.
        _ = builder.Services.AddLogging(loggingBuilder =>
        {
            _ = loggingBuilder.AddConsole();
            _ = loggingBuilder.AddDebug();
        });

        // Add services to the container.
        _ = builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            _ = options.UseInMemoryDatabase("DbInMemory");
        });

        _ = builder.Services.AddScoped<IRepositoryApplications, RepositoryApplications>();
        _ = builder.Services.AddScoped<IValidationService, ValidationService>();
        _ = builder.Services.AddScoped<IApplicationService, ApplicationService>();

        _ = builder.Services.AddControllers();

        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        _ = app.UseDefaultFiles();
        _ = app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
                options.RoutePrefix = "swagger";
            });
        }

        // Use CORS
        _ = app.UseCors();

        _ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        _ = app.MapFallbackToFile("/index.html");

        app.Run();
    }
}