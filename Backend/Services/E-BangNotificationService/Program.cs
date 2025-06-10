using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.ServiceExtensions;
using E_BangNotificationService.AppInfo;
using E_BangNotificationService.BackgroundWorker;
using E_BangNotificationService.Middleware;
using E_BangNotificationService.NotificationEntities;
using E_BangNotificationService.Repository;
using E_BangNotificationService.Service;
using E_BangNotificationService.SignalRHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


internal class Program
{
    private static async Task Main(string[] args)
    {
        bool isDocker = false;
        string? isDockerEnv = Environment.GetEnvironmentVariable("IS_DOCKER");
        if (isDockerEnv != null && isDockerEnv.Equals("true", StringComparison.CurrentCultureIgnoreCase))
        {
            isDocker = true;
        }
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<MigrationHandler>();
        builder.Services.AddSignalR();
        builder.Services.AddDbContext<NotificationDbContext>(options =>
        {
            string connectionString = isDocker ?
                Environment.GetEnvironmentVariable("EMAIL_CONNECTION_STRING")! :
                builder.Configuration.GetConnectionString("DbConnectionsString")!;
            options.UseNpgsql(connectionString);
        });
        builder.Services.AddSingleton<IInformations, Informations>();
        builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
        builder.Services.AddRabbitService();
        builder.Services.AddScoped<IPostgresDbRepostiory, PostgresDbRepostiory>();
        builder.Services.AddHostedService<NotificationWorker>();

        #region Options Pattern
        if (isDocker)
        {
            builder.Services.AddOptions<RabbitOptions>()
                .Configure(options =>
                {
                    options.Host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
                    options.Port = Convert.ToInt32(Environment.GetEnvironmentVariable("")!);
                    options.UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                    options.Password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD")!;
                    options.VirtualHost = Environment.GetEnvironmentVariable("RABBIT_VIRTUALHOST")!;
                    options.ListenerQueueName = Environment.GetEnvironmentVariable("RABBIT_NOTIFICATIONQUEUE")!;
                    options.SenderQueueName = "";
                });
        }
        else
        {
            builder.Services
                .AddOptions<RabbitOptions>()
                .ValidateOnStart()
                .BindConfiguration("RabbitOptions");
        }
        builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptions<RabbitOptions>>().Value);
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

        notification.MapGet("/", (string AccountID, bool IsRead, CancellationToken token) =>
        {
            return Results.Ok();
        });
        app.Run();
    }
}