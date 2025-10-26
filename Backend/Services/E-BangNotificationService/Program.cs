using App.RabbitBuilder.Options;
using App.RabbitBuilder.ServiceExtensions;
using AppInfo;
using CustomLogger.Abstraction;
using CustomLogger.Extension;
using CustomLogger.Model;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Middleware;
using NotificationEntities;
using Repository;
using SignalRHub;


internal class Program
{
    private static async Task Main(string[] args)
    {
        using var loggerFactory = new CustomLoggerFactory();
        ICustomLogger<Program> logger = loggerFactory.CreateLogger<Program>();
        try
        {
            bool isDocker = false;
            string? isDockerEnv = Environment.GetEnvironmentVariable("IS_DOCKER");
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCustomLogger();
            if (isDockerEnv != null && isDockerEnv.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                isDocker = true;
            }

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
                    builder.Configuration.GetConnectionString("DbConnectionString")!;
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddSingleton<IInformations, Informations>();
            builder.Services.AddRabbitService();
            builder.Services.AddScoped<IDbRepository, DbRepository>();
            builder.Services.AddQueues();
            #region Options Pattern
            if (isDocker)
            {
                builder.Services.AddOptions<RabbitOptionsExtended>()
                    .Configure(options =>
                    {
                        options.Host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
                        options.Port = Convert.ToInt32(Environment.GetEnvironmentVariable("")!);
                        options.UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        options.Password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD")!;
                        options.VirtualHost = Environment.GetEnvironmentVariable("RABBIT_VIRTUALHOST")!;
                        //options.ListenerQueueName = Environment.GetEnvironmentVariable("RABBIT_NOTIFICATIONQUEUE")!;
                        //options.SenderQueueName = "";
                    });
            }
            else
            {
                builder.Services
                    .AddOptions<RabbitOptionsExtended>()
                    .ValidateOnStart()
                    .BindConfiguration("RabbitOptions");
            }
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptions<RabbitOptionsExtended>>().Value);
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
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error ");
        }
        
    }
}