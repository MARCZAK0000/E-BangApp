using E_BangEmailWorker;
using E_BangEmailWorker.Database;
using E_BangEmailWorker.OptionsPattern;
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
            builder.Services.AddScoped<IDatabaseService, DatabaseService>();
            builder.Services.AddScoped<DatabaseSeed>();
            builder.Services.AddDbContext<ServiceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
            });
        #endregion

        #region OptionsPattern
            builder.Services.AddOptions<EmailConnectionOptions>()
                .ValidateOnStart()
                .BindConfiguration("EmailConnectionOptions");
            builder.Services.AddSingleton(pr => pr.GetRequiredService<IOptionsMonitor<EmailConnectionOptions>>().CurrentValue);

        #endregion
        builder.Services.AddHostedService<Worker>();
        var host = builder.Build();

        var scope = host.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
        await seeder.InvokeSeed();
        if (!await seeder.CheckConfigurationValues())
        {
            throw new ArgumentException("Invalid Configuration");
        }
        host.Run();
    }
}

