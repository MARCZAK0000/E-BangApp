using E_BangAppEmailBuilder.src.BuildersDto.Body;
using E_BangEmailWorker;
using E_BangEmailWorker.Database;
using E_BangEmailWorker.EmailMessageBuilderFactory;
using E_BangEmailWorker.EmailMessageBuilderFactory.BuilderOptions;
using E_BangEmailWorker.EmailMessageBuilderStrategy;
using E_BangEmailWorker.EmailMessageBuilderStrategy.BuilderOptions;
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
        var builder = Host.CreateApplicationBuilder(args);

        #region Service Registration
            builder.Services.AddScoped<IEmailServices, EmailServices>();
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IDatabaseService, DatabaseService>(); //Obsolete 
            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IRabbitQueueService, RabbitQueueService>();
        
            builder.Services.AddSingleton<DatabaseSeed>();
            builder.Services.AddDbContext<ServiceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
            });

        #endregion
        #region factoryPattern
            builder.Services.AddScoped<IEmailBuilderStrategy, EmailBuilderStrategy>();
            builder.Services.AddTransient<RegistrationEmailBuilder>();
            builder.Services.AddTransient<Func<object, IBuilderEmailBase>>(sp => key =>
            {
                return key switch
                {
                    RegistrationBodyBuilder => sp.GetRequiredService<RegistrationEmailBuilder>(),
                    _ => throw new NotImplementedException(),
                };
            });
        #endregion
        #region OptionsPattern
        builder.Services.AddOptions<EmailConnectionOptions>()
                .ValidateOnStart()
                .BindConfiguration("EmailConnectionOptions");
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptionsMonitor<EmailConnectionOptions>>().CurrentValue);

            builder.Services.AddOptions<RabbitOptions>()
                .ValidateOnStart()
                .BindConfiguration("RabbitOptions");
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptions<RabbitOptions>>().Value);

        #endregion
        builder.Services.AddHostedService<Worker>();
        var host = builder.Build();

        using var scope = host.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
        await seeder.InvokeSeed();
        if (!await seeder.CheckConfigurationValues())
        {
            throw new ArgumentException("Invalid Configuration");
        }
        host.Run();
    }
}

