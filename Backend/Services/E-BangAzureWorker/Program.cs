using System.Threading.Tasks;
using Azure.Storage.Blobs;
using E_BangAzureWorker;
using E_BangAzureWorker.Containers;
using E_BangAzureWorker.Database;
using E_BangAzureWorker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

internal class Program
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
            logger.LogInformation("Application started at {DateTime}", DateTime.Now);
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddSingleton<ContainerSeed>();
            builder.Services.AddSingleton<ContainerConnections>();
            builder.Services.AddSingleton(
                pr => new BlobServiceClient
                (builder.Configuration.GetConnectionString("BlobStorageConnectionString"),
                new BlobClientOptions(BlobClientOptions.ServiceVersion.V2020_02_10)));

            builder.Services.AddDbContext<ServiceDbContext>(x =>
                x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));

            builder.Services
                .AddOptions<ContainerSettings>()
                .BindConfiguration("ContainerSettings");
            builder.Services
                .AddSingleton<IContainerSettings>
                    (sp => sp.GetRequiredService<IOptions<ContainerSettings>>().Value);

            var host = builder.Build();
            using var scope = host.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<ContainerSeed>();
            var connections = scope.ServiceProvider.GetRequiredService<ContainerConnections>();
            await seeder.InvokeSeed();
            await connections.ValidateSettingsAsync();
            host.Run();
        }
        catch (Exception err)
        {
            logger.LogError(err.ToString());
        }
        
    }
}