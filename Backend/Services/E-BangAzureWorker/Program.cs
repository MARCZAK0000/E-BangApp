using Azure.Storage.Blobs;
using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangAzureWorker;
using E_BangAzureWorker.AzureBaseRepo;
using E_BangAzureWorker.AzureStrategy;
using E_BangAzureWorker.Azurite;
using E_BangAzureWorker.Containers;
using E_BangAzureWorker.Database;
using E_BangAzureWorker.DatabaseFactory;
using E_BangAzureWorker.DbRepository;
using E_BangAzureWorker.EventPublisher;
using E_BangAzureWorker.Model;
using E_BangAzureWorker.Repository;
using E_BangAzureWorker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class Program
{
    private static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole() // Log to the console
                .SetMinimumLevel(LogLevel.Information); // Set the minimum log level
        });

        // Create a logger
        var logger = loggerFactory.CreateLogger<Program>();
        try
        {
            bool isDocker = false;
            string? isDockerEnv = Environment.GetEnvironmentVariable("IS_DOCKER");
            if (isDockerEnv != null && isDockerEnv.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                isDocker = true;
            }
            logger.LogInformation("Application started at {DateTime}", DateTime.Now);
            var builder = Host.CreateApplicationBuilder(args);
            #region Services
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddSingleton<ContainerSeed>();
            builder.Services.AddSingleton<ContainerConnections>();
            builder.Services.AddSingleton<EmulatorInvoke>();
            builder.Services.AddScoped<IEventPublisher, EventPublisher>();
            builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
            builder.Services.AddScoped<IRabbitRepository, RabbitRepository>();
            string blobStorageConnection = isDocker? 
                Environment.GetEnvironmentVariable("BLOB_CONNECTION_STRING")!:
                builder.Configuration.GetConnectionString("BlobStorageConnectionString")!;
            builder.Services.AddSingleton(
                pr => new BlobServiceClient
                (blobStorageConnection,
                new BlobClientOptions(BlobClientOptions.ServiceVersion.V2020_02_10)));

            builder.Services.AddDbContext<ServiceDbContext>(x =>
            {
                string connectionString = isDocker? Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING")!:
                    builder.Configuration.GetConnectionString("DbConnectionString")!; ;
                x.UseSqlServer(builder.Configuration.GetConnectionString(connectionString));
            });
            builder.Services.AddScoped<IAzureStrategy, AzureStrategy>();
            builder.Services.AddScoped<AzureAddFileRepository>();
            builder.Services.AddScoped<AzureRemoveFileRepository>();
            builder.Services.AddScoped<Func<AzureStrategyEnum, IAzureBase>>(sp => key =>
            {
                return key switch
                {
                    AzureStrategyEnum.Add => sp.GetRequiredService<AzureAddFileRepository>(),
                    AzureStrategyEnum.Remove => sp.GetRequiredService<AzureRemoveFileRepository>(),
                    _ => throw new ArgumentException("Invalide Service"),
                };
            });
            builder.Services.AddScoped<IDbStrategy, DbStrategy>();
            builder.Services.AddScoped<DbAddFiles>();
            builder.Services.AddScoped<DbRemoveFIles>();
            builder.Services.AddScoped<Func<bool, IDbBase>>(sp => key =>
            {
                if (key)
                    return sp.GetRequiredService<DbRemoveFIles>();
                return sp.GetRequiredService<DbAddFiles>();
            });
            #endregion

            #region Options Pattern
            builder.Services
                .AddOptions<RabbitMQSettings>()
                .BindConfiguration("RabbitMQSetting");
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

            builder.Services
                .AddOptions<ContainerSettings>()
                .BindConfiguration("ContainerSettings");
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptions<ContainerSettings>>().Value);
            builder.Services
                .AddOptions<EmulatorSettings>()
                .BindConfiguration("EmulatorSettings");
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptions<EmulatorSettings>>().Value);
            #endregion
            var host = builder.Build();
            using var scope = host.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<ContainerSeed>();
            var connections = scope.ServiceProvider.GetRequiredService<ContainerConnections>();
            var emulator = scope.ServiceProvider.GetRequiredService<EmulatorInvoke>();
            await seeder.MirageAsync();
            await seeder.InvokeSeed();
            await connections.ValidateSettingsAsync();
            //emulator.RunEmulator();
            host.Run();
        }
        catch (Exception err)
        {
            logger.LogError("Error ocured at {Date}: {ex}", DateTime.Now, err.Message);
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

    }
}