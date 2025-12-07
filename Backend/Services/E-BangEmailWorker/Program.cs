using App.EmailBuilder.Extensions;
using App.EmailHelper.EmailTemplates.Body;
using App.EmailHelper.EmailTemplates.Footer;
using App.EmailHelper.EmailTemplates.Header;
using App.EmailRender.Shared.Abstraction;
using App.RabbitBuilder.Options;
using App.RabbitBuilder.ServiceExtensions;
using App.RenderEmail.Extensions;
using CustomLogger.Extension;
using CustomLogger.Model;
using E_BangEmailWorker;
using E_BangEmailWorker.Database;
using E_BangEmailWorker.OptionsPattern;
using E_BangEmailWorker.Repository;
using E_BangEmailWorker.Seeder;
using E_BangEmailWorker.Services;
using E_BangEmailWorker.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

public class Program
{
    private static async Task Main(string[] args)
    {
        using var loggerFactory = new CustomLoggerFactory();
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
            var emailOptions = new EmailConnectionOptions();
            builder.Configuration.GetSection("EmailConnectionOptions").Bind(emailOptions);
            #region Service Registration
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<GeneratorDatabaseSeed>();
            builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IRabbitQueueService, RabbitQueueService>();
            //builder.Services.AddHostedService<CleanerWorker>();
            builder.Services.AddCustomLogger();
            builder.Services.AddRenderEmailService();
            builder.Services.AddEmailSenderService(cfg =>
            {
                cfg.Port = emailOptions.Port;
                cfg.SmptHost = emailOptions.SmptHost;
                cfg.EmailName = emailOptions.EmailName;
                cfg.Password = emailOptions.Password;
            });
            builder.Services.AddRabbitService(cfg =>
            {
                cfg.ConnectionRetryCount = 3;
                cfg.ConnectionRetryDelaySeconds = 1;
                cfg.ServiceRetryCount = 5;
                cfg.ServiceRetryDelaySeconds = 2;
            });
            builder.Services.AddSingleton<DatabaseSeed>();
            builder.Services.AddDbContext<ServiceDbContext>(options =>
            {
                string connectionString = isDocker ?
                    Environment.GetEnvironmentVariable("EMAIL_CONNECTION_STRING")! :
                    builder.Configuration.GetConnectionString("DbConnectionString")!;
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddDbContext<GeneratorDbContext>(options =>
            {
                string connectionString = isDocker ?
                    Environment.GetEnvironmentVariable("GENERATOR_CONNECTION_STRING")! :
                    builder.Configuration.GetConnectionString("GenratorDbConnectionString")!;
                options.UseSqlite(connectionString);
            });
            ///Strategy
            ///
            builder.Services.AddScoped<IEmailTemplate, DefaultFooterTemplate>();
            builder.Services.AddScoped<IEmailTemplate, DefaultHeaderTemplate>();
            builder.Services.AddScoped<IEmailTemplate, RegistrationAccountTemplate>();
            builder.Services.AddScoped<IEmailTemplate, ConfirmEmailTemplate>();
            builder.Services.AddScoped<IEmailTemplate, TwoWayTokenTemplate>();

            builder.Services.AddScoped<DefaultFooterTemplate>();
            builder.Services.AddScoped<DefaultHeaderTemplate>();
            builder.Services.AddScoped<RegistrationAccountTemplate>();
            builder.Services.AddScoped<ConfirmEmailTemplate>();
            builder.Services.AddScoped<TwoWayTokenTemplate>();

            builder.Services.AddTransient<StrategyFactory>();
            ///
            #endregion
            #region OptionsPattern
            if (isDocker)
            {
                logger.LogInformation("Take info from ENV, Rabbit Options: {rabbit}", Environment.GetEnvironmentVariable("RABBIT_HOST"));
                builder.Services.AddOptions<RabbitOptions>()
                    .Configure(options =>
                    {
                        options.Host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
                        options.Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBIT_PORT")!);
                        options.UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        options.Password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD")!;
                        options.VirtualHost = Environment.GetEnvironmentVariable("RABBIT_VIRTUALHOST")!;
                        options.ListenerQueueName = JsonSerializer.Deserialize<QueueOptions>(Environment.GetEnvironmentVariable("RABBIT_LISTENERQUEUENAME")!)!;
                        options.SenderQueueName = new();
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
            builder.Services.AddSingleton(emailOptions);

            #endregion
            builder.Services.AddHostedService<Worker>();
            var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
            await seeder.MigrateAsync();
            await seeder.InvokeSeed();
            await seeder.CheckConfigurationValues();
            var generatorSeeder= scope.ServiceProvider.GetRequiredService<GeneratorDatabaseSeed>();
            generatorSeeder.Migrate();
            host.Run();
        }
        catch (Exception err)
        {
            logger.LogError(err, "Error ocured, {0}",err.Message);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}

