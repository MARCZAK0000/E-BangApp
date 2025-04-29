using E_BangNotificationService.AppInfo;
using E_BangNotificationService.BackgroundWorker;
using E_BangNotificationService.Middleware;
using E_BangNotificationService.NotificationEntities;
using E_BangNotificationService.SignalRHub;
using Microsoft.EntityFrameworkCore;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<MigrationHandler>();
        builder.Services.AddSignalR();
        builder.Services.AddDbContext<NotificationDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DbConnectionsString")!;
            options.UseNpgsql(connectionString);
        });
        builder.Services.AddSingleton<IInformations, Informations>();
        builder.Services.AddHostedService<NotificationWorker>();
        var app = builder.Build();
        using var scope = app.Services.CreateScope();
        var migration = scope.ServiceProvider.GetRequiredService<MigrationHandler>();
        migration.MigrateDb();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Version one");
            });
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapHub<NotificationHub>("hub/notification");

        app.UseMiddleware<ErrorHandlingMiddleware>();

        var health = app.MapGroup("api/health");

        //health.MapGet("/")
        app.Run();
    }
}