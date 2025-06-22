using E_BangAppEmailBuilder.src.Abstraction;
using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.ServiceExtensions;
using E_BangEmailWorker;
using E_BangEmailWorker.Database;
using E_BangEmailWorker.OptionsPattern;
using E_BangEmailWorker.Repository;
using E_BangEmailWorker.Seeder;
using E_BangEmailWorker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class Program
{
    private static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(build =>
        {
            build
                .ClearProviders()
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });
        var logger = loggerFactory.CreateLogger<Program>(); 
        try
        {
            bool isDocker = false;
            string? isDockerEnv = Environment.GetEnvironmentVariable("IS_DOCKER");
            if (isDockerEnv != null && isDockerEnv.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                isDocker = true;
            }
            var builder = Host.CreateApplicationBuilder(args);
            #region Service Registration
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IRabbitQueueService, RabbitQueueService>();
            builder.Services.AddScoped<IBuilderEmail, BuilderEmail>();
            builder.Services.AddRabbitService();
            builder.Services.AddSingleton<DatabaseSeed>();
            builder.Services.AddDbContext<ServiceDbContext>(options =>
            {
                string connectionString = isDocker? 
                    Environment.GetEnvironmentVariable("EMAIL_CONNECTION_STRING")!:
                    builder.Configuration.GetConnectionString("DbConnectionString")!;
                options.UseSqlServer(connectionString);
            });

            #endregion
            #region OptionsPattern
            builder.Services.AddOptions<EmailConnectionOptions>()
                    .ValidateOnStart()
                    .BindConfiguration("EmailConnectionOptions");
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptionsMonitor<EmailConnectionOptions>>().CurrentValue);
            if (isDocker)
            {
                logger.LogInformation("{Date}: Take info from ENV, Rabbit Options: {rabbit}", DateTime.Now, Environment.GetEnvironmentVariable("RABBIT_HOST"));
                builder.Services.AddOptions<RabbitOptions>()
                    .Configure(options =>
                    {
                        options.Host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
                        options.Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBIT_PORT")!);
                        options.UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        options.Password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD")!;
                        options.VirtualHost = Environment.GetEnvironmentVariable("RABBIT_VIRTUALHOST")!;
                        options.ListenerQueueName = Environment.GetEnvironmentVariable("RABBIT_EMAILQUEUE")!;
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
            builder.Services.AddHostedService<Worker>();
            var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
            await seeder.MigrateAsync();
            await seeder.InvokeSeed();
            if (!await seeder.CheckConfigurationValues())
            {
                throw new ArgumentException("Invalid Configuration");
            }
            host.Run();
        }
        catch (Exception err)
        {
            logger.LogError("Error ocured at {Date}: {ex}", DateTime.Now, err.Message);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}

