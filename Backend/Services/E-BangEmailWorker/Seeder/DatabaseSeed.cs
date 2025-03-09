using E_BangEmailWorker.Database;
using E_BangEmailWorker.OptionsPattern;
using E_BangEmailWorker.Services;
using Microsoft.EntityFrameworkCore;

namespace E_BangEmailWorker.Seeder
{
    public class DatabaseSeed
    {
        private readonly ServiceDbContext _serviceDbContext;
        private readonly EmailConnectionOptions _connectionOptions;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<DatabaseSeed> _logger;
        private readonly IServiceProvider _serviceProvider;
        public DatabaseSeed(IServiceProvider serviceProvider,
            EmailConnectionOptions connectionOptions,
            IPasswordHasher passwordHasher,
            ILogger<DatabaseSeed> logger)
        {
            _serviceProvider = serviceProvider;
            _connectionOptions = connectionOptions;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _serviceDbContext = serviceProvider.GetRequiredService<ServiceDbContext>();
        }

        public async Task InvokeSeed()
        {
            if (await _serviceDbContext.Database.CanConnectAsync())
            {
                if (!await _serviceDbContext.EmailSettings.AnyAsync())
                {
                    await _serviceDbContext.EmailSettings.AddAsync(new EmailSettings()
                    {
                        EmailName = _connectionOptions.EmailName,
                        Password = _passwordHasher.GeneratePasswordHash(_connectionOptions.Password, _connectionOptions.Salt),
                        Port = _connectionOptions.Port,
                        SmptHost = _connectionOptions.SmptHost,
                        Salt = _connectionOptions.Salt
                    });
                    await _serviceDbContext.SaveChangesAsync();
                }
            }
            _logger.LogInformation("DbConfiguration Initalized");
        }
        public async Task<bool> CheckConfigurationValues()
        {

            if (!await _serviceDbContext.Database.CanConnectAsync())
            {
                return false;
            }
            var configuration = await _serviceDbContext.EmailSettings.FirstOrDefaultAsync();
            if (configuration is null)
            {
                return false;
            }
            if (!_connectionOptions.Equals(configuration))
            {
                return false;
            }
            if (!_passwordHasher.VerifyPassword(_connectionOptions.Password, configuration.Password, configuration.Salt))
            {
                configuration.Salt = _connectionOptions.Salt;
                configuration.Password = _passwordHasher.GeneratePasswordHash(_connectionOptions.Password, _connectionOptions.Salt);
                configuration.LastUpdateTime = DateTime.Now;
                await _serviceDbContext.SaveChangesAsync();
                _logger.LogWarning("DbConfiguration: Password changed");
            }
            _logger.LogInformation("DbConfiguration: Configuration correct");
            return true;
        }
    }
}