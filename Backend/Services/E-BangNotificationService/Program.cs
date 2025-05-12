using E_BangNotificationService.AppInfo;
using E_BangNotificationService.BackgroundWorker;
using E_BangNotificationService.Middleware;
using E_BangNotificationService.NotificationEntities;
using E_BangNotificationService.OptionsPattern;
using E_BangNotificationService.Repository;
using E_BangNotificationService.Service;
using E_BangNotificationService.SignalRHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


internal class Program
{
    private static async Task Main(string[] args)
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
        builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
        builder.Services.AddScoped<IRabbitRepository, RabbitRepository>();
        builder.Services.AddScoped<IPostgresDbRepostiory, PostgresDbRepostiory>();   
        builder.Services.AddHostedService<NotificationWorker>();

        #region Options Pattern
        builder.Services.AddOptions<RabbitOptions>()
            .ValidateDataAnnotations()
            .BindConfiguration("RabbitOptions")
            .ValidateOnStart();
        builder.Services.AddSingleton(sp=>sp.GetRequiredService<IOptions<RabbitOptions>>().Value);
        #endregion
        var app = builder.Build();
        using var scope = app.Services.CreateScope();
        var migration = scope.ServiceProvider.GetRequiredService<MigrationHandler>();
        await migration.MigrateDb();
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
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.MapHub<NotificationHub>("hub/notification");

        var health = app.MapGroup("api/health");
        var notification = app.MapGroup("api/notification");

        health.MapGet("/", (IInformations informations) =>
        {
            return Results.Ok(informations);
        });

        notification.MapGet("/", async (string AccountID, bool IsRead, CancellationToken token) =>
        {
            return Results.Ok();
        });


        app.Run();
    }
}