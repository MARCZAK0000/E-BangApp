using E_BangDomain.Entities;
using E_BangInfrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_BangInfrastructure.Repository
{
    public class AccountRepository(UserManager<Account> userManager, 
        IUserStore<Account> userStore, 
        ProjectDbContext projectDbContext, 
        ILogger<AccountRepository> logger)
    {
        private readonly UserManager<Account> _userManager = userManager;

        private readonly IUserStore<Account> _userStore = userStore;

        private readonly ProjectDbContext projectDbContext = projectDbContext;
        
        private readonly ILogger<AccountRepository> _logger = logger;

        public async Task<bool> RegisterAccountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
