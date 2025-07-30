using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangInfrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace E_BangInfrastructure.Repository
{
    public sealed class AccountRepository(UserManager<Account> userManager,
        //IUserStore<Account> userStore,
        ProjectDbContext projectDbContext,
        SignInManager<Account> signInManager
        //IUserEmailStore<Account> userEmailStore
        ) : IAccountRepository
    {
        private readonly UserManager<Account> _userManager = userManager;

        private readonly SignInManager<Account> _signInManager = signInManager;

        //private readonly IUserStore<Account> _userStore = userStore;

        //private readonly IUserEmailStore<Account> _userEmailStore = userEmailStore;

        private readonly ProjectDbContext _projectDbContext = projectDbContext;


        public async Task<bool> RegisterAccountAsync(Account account,string password)
        {
            IdentityResult result = await _userManager.CreateAsync(account, password);
            return result.Succeeded;
        }
        public async Task<Maybe<Account>> FindAccountByEmailAsync(string email, CancellationToken token)
        {
            Account? user = await _userManager.FindByEmailAsync(email);
            return new Maybe<Account>(user);
        }
        public async Task<bool> ValidateLoginCredentialsAsync(Account user, LoginAccountDto login)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            return result.Succeeded;
        }

        public async Task<bool> ValidateLoginWithTwoWayFactoryCodeAsync(Account user, LoginAccountDto login)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            return signInResult.Succeeded && user.TwoFactoryCode.Equals(login.TwoFactorCode, StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task<string> GenerateConfirmEmailTokenAsync(Account user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }
        public async Task<bool> ConfirmEmailAsync(Account account, string confirmToken)
        {
            string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmToken));
            IdentityResult result = await _userManager.ConfirmEmailAsync(account, decodedToken);
            return result.Succeeded;
        }

        public async Task<Maybe<Account>> FindAccountByIdAsync(string accountId)
        {
            Account? user = await _userManager.FindByIdAsync(accountId);
            return new Maybe<Account>(user);
        }

        public async Task<string> GenerateForgetPasswordTokenAsync(Account account)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(account);
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes((string)token));
        }

        public async Task<bool> SetNewPasswordAsync(Account account, string newPassword, string token)
        {
            IdentityResult result = await _userManager.ResetPasswordAsync(account, newPassword, token);

            return result.Succeeded;
        }
        public async Task<bool> ChangePasswordAsync(Account account, string oldPassword, string newPassword)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(account, oldPassword, newPassword);
            return result.Succeeded;
        }
        public async Task<string> GetRefreshTokenAsync(string accountId, CancellationToken token)
            => await _projectDbContext.Account.Where(pr => pr.Id == accountId)
                .Select(pr => pr.RefreshToken).FirstOrDefaultAsync(token) ?? string.Empty;

        public async Task<bool> LastUdateTimeAsync(string accountID, CancellationToken token) =>
            await _projectDbContext
            .Account
            .Where(pr => pr.Id == accountID)
                .ExecuteUpdateAsync(pr => pr.SetProperty(p => p.LastUpdateTime, p => DateTime.Now), token) > 0;

        public async Task<string> GetAccountIdFromEmail(string email, CancellationToken token)
            => await _projectDbContext.Account
                .Where(pr => pr.Email == email)
                .Select(pr => pr.Id)
                .FirstOrDefaultAsync(token) ?? string.Empty;
    }
}
