using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangInfrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_BangInfrastructure.Repository
{
    public class AccountRepository(UserManager<Account> userManager, 
        IUserStore<Account> userStore, 
        ProjectDbContext projectDbContext,
        SignInManager<Account> signInManager,
        IUserEmailStore<Account> userEmailStore,
        ILogger<AccountRepository> logger) : IAccountRepository
    {
        private readonly UserManager<Account> _userManager = userManager;

        private readonly SignInManager<Account> _signInManager = signInManager;

        private readonly IUserStore<Account> _userStore = userStore;

        private readonly IUserEmailStore<Account> _userEmailStore = userEmailStore;

        private readonly ProjectDbContext projectDbContext = projectDbContext;
        
        private readonly ILogger<AccountRepository> _logger = logger;

        public async Task<Account> RegisterAccountAsync(RegisterAccountDto registerAccountDto, CancellationToken token)
        {
            Account user = new();
            await _userStore.SetUserNameAsync(user, registerAccountDto.Email, token);
            await _userStore.SetNormalizedUserNameAsync(user, registerAccountDto.Email, token);
            await _userEmailStore.SetEmailAsync(user, registerAccountDto.Email, token);
            await _userManager.CreateAsync(user, registerAccountDto.Password);
            return user;
        }
        public Task<Maybe<Account>> FindAccountByEmailAsync(string email, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateLoginCredentialsAsync(Account user, LoginAccountDto login)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            return result.Succeeded;
        }

        public async Task<bool> ValidateLoginWithTwoWayFactoryCodeAsync(Account user, LoginAccountDto login)
        {
            bool result = false;
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if(signInResult.Succeeded)
            {
                
            }
            return result;
        }
        public Task<bool> LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
